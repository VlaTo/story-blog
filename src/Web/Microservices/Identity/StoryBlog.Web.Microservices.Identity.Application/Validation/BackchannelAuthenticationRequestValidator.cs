using System.Collections.Specialized;
using IdentityModel;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Microservices.Identity.Application.Configuration;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Application.Models.Requests;
using StoryBlog.Web.Microservices.Identity.Application.Services;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Contexts;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Requests;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Results;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation;

internal sealed class BackchannelAuthenticationRequestValidator : IBackchannelAuthenticationRequestValidator
{
    private readonly IdentityServerOptions options;
    private readonly IResourceValidator resourceValidator;
    private readonly ITokenValidator tokenValidator;
    private readonly IBackchannelAuthenticationUserValidator backchannelAuthenticationUserValidator;
    private readonly IJwtRequestValidator jwtRequestValidator;
    private readonly IJwtRequestUriHttpClient jwtRequestUriHttpClient;
    private readonly ILogger<BackchannelAuthenticationRequestValidator> logger;

    private ValidatedBackchannelAuthenticationRequest validatedRequest;

    public BackchannelAuthenticationRequestValidator(
        IdentityServerOptions options,
        IResourceValidator resourceValidator,
        ITokenValidator tokenValidator,
        IBackchannelAuthenticationUserValidator backchannelAuthenticationUserValidator,
        IJwtRequestValidator jwtRequestValidator,
        IJwtRequestUriHttpClient jwtRequestUriHttpClient,
        ILogger<BackchannelAuthenticationRequestValidator> logger)
    {
        this.options = options;
        this.resourceValidator = resourceValidator;
        this.tokenValidator = tokenValidator;
        this.backchannelAuthenticationUserValidator = backchannelAuthenticationUserValidator;
        this.jwtRequestValidator = jwtRequestValidator;
        this.jwtRequestUriHttpClient = jwtRequestUriHttpClient;
        this.logger = logger;
    }

    public async Task<BackchannelAuthenticationRequestValidationResult> ValidateRequestAsync(NameValueCollection parameters, ClientSecretValidationResult clientValidationResult)
    {
        using var activity = Tracing.BasicActivitySource.StartActivity("BackchannelAuthenticationRequestValidator.ValidateRequest");

        if (null == clientValidationResult)
        {
            throw new ArgumentNullException(nameof(clientValidationResult));
        }

        logger.LogDebug("Start backchannel authentication request validation");

        validatedRequest = new ValidatedBackchannelAuthenticationRequest
        {
            Raw = parameters ?? throw new ArgumentNullException(nameof(parameters)),
            Options = options
        };
        validatedRequest.SetClient(
            clientValidationResult.Client,
            clientValidationResult.Secret,
            clientValidationResult.Confirmation
        );

        //////////////////////////////////////////////////////////
        // Client must be configured for CIBA
        //////////////////////////////////////////////////////////
        if (false == clientValidationResult.Client?.AllowedGrantTypes.Contains(OidcConstants.GrantTypes.Ciba))
        {
            LogError("Client {clientId} not configured with the CIBA grant type.", clientValidationResult.Client.ClientId);
            return Invalid(OidcConstants.BackchannelAuthenticationRequestErrors.UnauthorizedClient, "Unauthorized client");
        }

        //LicenseValidator.ValidateCiba();

        //////////////////////////////////////////////////////////
        // load request object
        //////////////////////////////////////////////////////////
        var jwtRequest = validatedRequest.Raw.Get(OidcConstants.BackchannelAuthenticationRequest.Request);

        // check length restrictions
        if (jwtRequest.IsPresent())
        {
            if (options.InputLengthRestrictions.Jwt <= jwtRequest.Length)
            {
                LogError("request value is too long");
                return Invalid(OidcConstants.AuthorizeErrors.InvalidRequestObject, "Invalid request value");
            }
        }

        validatedRequest.RequestObject = jwtRequest;

        //////////////////////////////////////////////////////////
        // validate request object
        //////////////////////////////////////////////////////////
        var roValidationResult = await TryValidateRequestObjectAsync();
        
        if (false == roValidationResult.Success)
        {
            return roValidationResult.ErrorResult;
        }

        if (validatedRequest.Client.RequireRequestObject && false == validatedRequest.RequestObjectValues.Any())
        {
            LogError("Client is configured for RequireRequestObject but none present");
            return Invalid(OidcConstants.BackchannelAuthenticationRequestErrors.InvalidRequest);
        }

        //////////////////////////////////////////////////////////
        // scope must be present
        //////////////////////////////////////////////////////////
        var scope = validatedRequest.Raw.Get(OidcConstants.BackchannelAuthenticationRequest.Scope);

        if (scope.IsMissing())
        {
            LogError("Missing scope");
            return Invalid(OidcConstants.BackchannelAuthenticationRequestErrors.InvalidRequest, "Missing scope");
        }

        if (options.InputLengthRestrictions.Scope < scope.Length)
        {
            LogError("scopes too long.");
            return Invalid(OidcConstants.BackchannelAuthenticationRequestErrors.InvalidRequest, "Invalid scope");
        }

        validatedRequest.RequestedScopes = scope.FromSpaceSeparatedString().Distinct().ToList();

        //////////////////////////////////////////////////////////
        // openid scope required
        //////////////////////////////////////////////////////////
        if (false == validatedRequest.RequestedScopes.Contains(IdentityServerConstants.StandardScopes.OpenId))
        {
            LogError("openid scope missing.");
            return Invalid(OidcConstants.BackchannelAuthenticationRequestErrors.InvalidRequest, "Missing the openid scope");
        }

        //////////////////////////////////////////////////////////
        // check for resource indicators and valid format
        //////////////////////////////////////////////////////////
        var resourceIndicators = validatedRequest.Raw.GetValues(OidcConstants.AuthorizeRequest.Resource) ?? Enumerable.Empty<string>();

        if (resourceIndicators.Any(x => options.InputLengthRestrictions.ResourceIndicatorMaxLength < x.Length))
        {
            return Invalid(OidcConstants.BackchannelAuthenticationRequestErrors.InvalidTarget, "Resource indicator maximum length exceeded");
        }

        if (false == resourceIndicators.AreValidResourceIndicatorFormat(logger))
        {
            return Invalid(OidcConstants.BackchannelAuthenticationRequestErrors.InvalidTarget, "Invalid resource indicator format");
        }

        validatedRequest.RequestedResourceIndicators = resourceIndicators.ToList();

        //////////////////////////////////////////////////////////
        // check if scopes are valid/supported and check for resource scopes
        //////////////////////////////////////////////////////////
        var validatedResources = await resourceValidator.ValidateRequestedResourcesAsync(new ResourceValidationRequest
        {
            Client = validatedRequest.Client,
            Scopes = validatedRequest.RequestedScopes,
            ResourceIndicators = resourceIndicators
        });

        if (false == validatedResources.Succeeded)
        {
            if (validatedResources.InvalidResourceIndicators.Any())
            {
                return Invalid(OidcConstants.BackchannelAuthenticationRequestErrors.InvalidTarget, "Invalid resource indicator");
            }

            if (validatedResources.InvalidScopes.Any())
            {
                return Invalid(OidcConstants.BackchannelAuthenticationRequestErrors.InvalidScope, "Invalid scope");
            }
        }

        //LicenseValidator.ValidateResourceIndicators(resourceIndicators);
        validatedRequest.ValidatedResources = validatedResources;


        //////////////////////////////////////////////////////////
        // check requested_expiry
        //////////////////////////////////////////////////////////
        var requestLifetime = validatedRequest.Client.CibaLifetime ?? options.Ciba.DefaultLifetime;
        var requestedExpiry = validatedRequest.Raw.Get(OidcConstants.BackchannelAuthenticationRequest.RequestedExpiry);
        
        if (requestedExpiry.IsPresent())
        {
            // Int32.MaxValue == 2147483647, which is 10 characters in length
            // so using 9 so we don't overflow below on the Int32.Parse
            if (9 < requestedExpiry.Length)
            {
                LogError("requested_expiry too long");
                return Invalid(OidcConstants.BackchannelAuthenticationRequestErrors.InvalidRequest, "Invalid requested_expiry");
            }

            if (Int32.TryParse(requestedExpiry, out var expiryValue))
            {
                var expiry = TimeSpan.FromSeconds(expiryValue);

                if (TimeSpan.Zero < expiry && expiry <= requestLifetime)
                {
                    validatedRequest.Expiry = expiry;
                }
                else
                {
                    LogError("requested_expiry value out of valid range");
                    return Invalid(OidcConstants.BackchannelAuthenticationRequestErrors.InvalidRequest, "Invalid requested_expiry");
                }
            }
        }
        else
        {
            validatedRequest.Expiry = requestLifetime;
        }

        //////////////////////////////////////////////////////////
        // check acr_values
        //////////////////////////////////////////////////////////
        var acrValues = validatedRequest.Raw.Get(OidcConstants.BackchannelAuthenticationRequest.AcrValues);

        if (acrValues.IsPresent())
        {
            if (options.InputLengthRestrictions.AcrValues < acrValues.Length)
            {
                LogError("Acr values too long");
                return Invalid(OidcConstants.BackchannelAuthenticationRequestErrors.InvalidRequest, "Invalid acr_values");
            }

            validatedRequest.AuthenticationContextReferenceClasses = acrValues.FromSpaceSeparatedString().Distinct().ToList();

            //////////////////////////////////////////////////////////
            // check custom acr_values: idp and tenant
            //////////////////////////////////////////////////////////
            var tenant = validatedRequest.AuthenticationContextReferenceClasses.FirstOrDefault(x => x.StartsWith(Constants.KnownAcrValues.Tenant));
            
            if (null != tenant)
            {
                validatedRequest.AuthenticationContextReferenceClasses.Remove(tenant);
                tenant = tenant.Substring(Constants.KnownAcrValues.Tenant.Length);
                validatedRequest.Tenant = tenant;
            }

            var idp = validatedRequest.AuthenticationContextReferenceClasses.FirstOrDefault(x => x.StartsWith(Constants.KnownAcrValues.HomeRealm));
            
            if (null != idp)
            {
                validatedRequest.AuthenticationContextReferenceClasses.Remove(idp);

                idp = idp.Substring(Constants.KnownAcrValues.HomeRealm.Length);

                // check if idp is present but client does not allow it, and then ignore it
                if (null != validatedRequest.Client.IdentityProviderRestrictions &&
                    validatedRequest.Client.IdentityProviderRestrictions.Any())
                {
                    if (false == validatedRequest.Client.IdentityProviderRestrictions.Contains(idp))
                    {
                        logger.LogWarning("idp requested ({idp}) is not in client restriction list.", idp);
                        idp = null;
                    }
                }

                validatedRequest.IdP = idp;
            }
        }


        //////////////////////////////////////////////////////////
        // login hints
        //////////////////////////////////////////////////////////
        var loginHint = validatedRequest.Raw.Get(OidcConstants.BackchannelAuthenticationRequest.LoginHint);
        var loginHintToken = validatedRequest.Raw.Get(OidcConstants.BackchannelAuthenticationRequest.LoginHintToken);
        var idTokenHint = validatedRequest.Raw.Get(OidcConstants.BackchannelAuthenticationRequest.IdTokenHint);

        var loginHintCount = 0;

        if (loginHint.IsPresent())
        {
            loginHintCount++;
        }

        if (loginHintToken.IsPresent())
        {
            loginHintCount++;
        }

        if (idTokenHint.IsPresent())
        {
            loginHintCount++;
        }

        if (0 == loginHintCount)
        {
            LogError("Missing login_hint_token, id_token_hint, or login_hint");
            return Invalid(OidcConstants.BackchannelAuthenticationRequestErrors.InvalidRequest, "Missing login_hint_token, id_token_hint, or login_hint");
        }

        if (1 < loginHintCount)
        {
            LogError("Too many of login_hint_token, id_token_hint, or login_hint");
            return Invalid(OidcConstants.BackchannelAuthenticationRequestErrors.InvalidRequest, "Too many of login_hint_token, id_token_hint, or login_hint");
        }

        //////////////////////////////////////////////////////////
        // check login_hint
        //////////////////////////////////////////////////////////
        if (loginHint.IsPresent())
        {
            if (options.InputLengthRestrictions.LoginHint < loginHint.Length)
            {
                LogError("Login hint too long");
                return Invalid(OidcConstants.BackchannelAuthenticationRequestErrors.InvalidRequest, "Invalid login_hint");
            }

            validatedRequest.LoginHint = loginHint;
        }

        //////////////////////////////////////////////////////////
        // check login_hint_token
        //////////////////////////////////////////////////////////
        if (loginHintToken.IsPresent())
        {
            if (options.InputLengthRestrictions.LoginHintToken < loginHintToken.Length)
            {
                LogError("Login hint token too long");
                return Invalid(OidcConstants.BackchannelAuthenticationRequestErrors.InvalidRequest, "Invalid login_hint_token");
            }

            validatedRequest.LoginHintToken = loginHintToken;
        }

        //////////////////////////////////////////////////////////
        // check id_token_hint
        //////////////////////////////////////////////////////////
        if (idTokenHint.IsPresent())
        {
            if (options.InputLengthRestrictions.IdTokenHint < idTokenHint.Length)
            {
                LogError("id token hint too long");
                return Invalid(OidcConstants.BackchannelAuthenticationRequestErrors.InvalidRequest, "Invalid id_token_hint");
            }

            var idTokenHintValidationResult = await tokenValidator.ValidateIdentityTokenAsync(idTokenHint, validatedRequest.ClientId, false);
            
            if (idTokenHintValidationResult.IsError)
            {
                LogError("id token hint failed to validate: " + idTokenHintValidationResult.Error, idTokenHintValidationResult.ErrorDescription);
                return Invalid(OidcConstants.BackchannelAuthenticationRequestErrors.InvalidRequest, "Invalid id_token_hint");
            }

            validatedRequest.IdTokenHint = idTokenHint;
            validatedRequest.IdTokenHintClaims = idTokenHintValidationResult.Claims;
        }

        //////////////////////////////////////////////////////////
        // check user_code
        //////////////////////////////////////////////////////////
        var userCode = validatedRequest.Raw.Get(OidcConstants.BackchannelAuthenticationRequest.UserCode);
        
        if (userCode.IsPresent())
        {
            if (options.InputLengthRestrictions.UserCode < userCode.Length)
            {
                LogError("user_code too long");
                return Invalid(OidcConstants.BackchannelAuthenticationRequestErrors.InvalidRequest, "Invalid user_code");
            }

            validatedRequest.UserCode = userCode;
        }

        //////////////////////////////////////////////////////////
        // check binding_message
        //////////////////////////////////////////////////////////
        var bindingMessage = validatedRequest.Raw.Get(OidcConstants.BackchannelAuthenticationRequest.BindingMessage);
        
        if (bindingMessage.IsPresent())
        {
            if (options.InputLengthRestrictions.BindingMessage < bindingMessage.Length)
            {
                LogError("binding_message too long");
                return Invalid(OidcConstants.BackchannelAuthenticationRequestErrors.InvalidBindingMessage, "Invalid binding_message");
            }

            validatedRequest.BindingMessage = bindingMessage;
        }

        //////////////////////////////////////////////////////////
        // validate the login hint w/ custom validator
        //////////////////////////////////////////////////////////
        var userResult = await backchannelAuthenticationUserValidator.ValidateRequestAsync(new BackchannelAuthenticationUserValidatorContext
        {
            Client = validatedRequest.Client,
            IdTokenHint = validatedRequest.IdTokenHint,
            LoginHint = validatedRequest.LoginHint,
            LoginHintToken = validatedRequest.LoginHintToken,
            IdTokenHintClaims = validatedRequest.IdTokenHintClaims,
            UserCode = validatedRequest.UserCode,
            BindingMessage = validatedRequest.BindingMessage
        });

        if (userResult.IsError)
        {
            if (userResult.Error == OidcConstants.BackchannelAuthenticationRequestErrors.AccessDenied)
            {
                LogError("Request was denied access for that user");
                return Invalid(OidcConstants.BackchannelAuthenticationRequestErrors.AccessDenied, userResult.ErrorDescription);
            }
            
            if (userResult.Error == OidcConstants.BackchannelAuthenticationRequestErrors.ExpiredLoginHintToken)
            {
                LogError("Expired login_hint_token");
                return Invalid(OidcConstants.BackchannelAuthenticationRequestErrors.ExpiredLoginHintToken, userResult.ErrorDescription ?? "Expired login_hint_token");
            }
            
            if (userResult.Error == OidcConstants.BackchannelAuthenticationRequestErrors.UnknownUserId)
            {
                LogError("Unknown user id");
                return Invalid(OidcConstants.BackchannelAuthenticationRequestErrors.UnknownUserId, userResult.ErrorDescription);
            }
            
            if (userResult.Error == OidcConstants.BackchannelAuthenticationRequestErrors.MissingUserCode)
            {
                LogError("Missing user_code");
                return Invalid(OidcConstants.BackchannelAuthenticationRequestErrors.MissingUserCode, userResult.ErrorDescription);
            }
            
            if (userResult.Error == OidcConstants.BackchannelAuthenticationRequestErrors.InvalidUserCode)
            {
                LogError("Invalid user_code");
                return Invalid(OidcConstants.BackchannelAuthenticationRequestErrors.InvalidUserCode, userResult.ErrorDescription);
            }
            
            if (userResult.Error == OidcConstants.BackchannelAuthenticationRequestErrors.InvalidBindingMessage)
            {
                LogError("Invalid binding_message");
                return Invalid(OidcConstants.BackchannelAuthenticationRequestErrors.InvalidBindingMessage, userResult.ErrorDescription);
            }

            LogError("Unexpected error from IBackchannelAuthenticationUserValidator: {error}", userResult.Error);
            return Invalid(OidcConstants.BackchannelAuthenticationRequestErrors.UnknownUserId);
        }

        if (null == userResult.Subject || !userResult.Subject.HasClaim(x => x.Type == JwtClaimTypes.Subject))
        {
            LogError("No subject or subject id returned from IBackchannelAuthenticationUserValidator");
            return Invalid(OidcConstants.BackchannelAuthenticationRequestErrors.UnknownUserId);
        }

        validatedRequest.Subject = userResult.Subject;

        LogSuccess();

        return new BackchannelAuthenticationRequestValidationResult(validatedRequest);
    }

    private async Task<(bool Success, BackchannelAuthenticationRequestValidationResult? ErrorResult)> TryValidateRequestObjectAsync()
    {
        //////////////////////////////////////////////////////////
        // validate request object
        /////////////////////////////////////////////////////////
        if (validatedRequest.RequestObject.IsPresent())
        {
            // validate the request JWT for this client
            var jwtRequestValidationResult = await jwtRequestValidator.ValidateAsync(new JwtRequestValidationContext
            {
                Client = validatedRequest.Client,
                JwtTokenString = validatedRequest.RequestObject,
                StrictJarValidation = false,
                IncludeJti = true
            });
            if (jwtRequestValidationResult.IsError)
            {
                LogError("request JWT validation failure", jwtRequestValidationResult.Error);
                return (false, Invalid(OidcConstants.AuthorizeErrors.InvalidRequestObject, "Invalid JWT request"));
            }

            // client_id not required in JWT, but just in case we will validate it
            var payloadClientId = jwtRequestValidationResult.Payload.SingleOrDefault(x => x.Type == JwtClaimTypes.ClientId)?.Value;
            
            if (payloadClientId.IsPresent() && payloadClientId != validatedRequest.Client?.ClientId)
            {
                var temp = new
                {
                    invalidClientId = payloadClientId, 
                    clientId = validatedRequest.Client?.ClientId
                };

                LogError("client_id found in the JWT request object does not match client_id used to authenticate", temp);
                
                return (false, Invalid(OidcConstants.AuthorizeErrors.InvalidRequestObject, "Invalid client_id in JWT request"));
            }

            // validate jti in request token
            var jti = jwtRequestValidationResult.Payload
                .SingleOrDefault(x => x.Type == JwtClaimTypes.JwtId)?
                .Value;
            
            if (jti.IsMissing())
            {
                LogError("Missing jti in JWT request object");
                return (false, Invalid(OidcConstants.AuthorizeErrors.InvalidRequestObject, "Missing jti in JWT request object"));
            }

            // validate that no request params are in body, and merge them into the request collection
            foreach (var claim in jwtRequestValidationResult.Payload)
            {
                // we already checked client_id above
                if (JwtClaimTypes.ClientId != claim.Type)
                {
                    if (validatedRequest.Raw.AllKeys.Contains(claim.Type))
                    {
                        LogError("Parameter from JWT request object also found in request body: {name}", claim.Type);
                        return (false, Invalid(OidcConstants.AuthorizeErrors.InvalidRequestObject, "Parameter from JWT request object also found in request body"));
                    }

                    if (claim.Type != JwtClaimTypes.JwtId)
                    {
                        validatedRequest.Raw.Add(claim.Type, claim.Value);
                    }
                }
            }

            validatedRequest.RequestObjectValues = jwtRequestValidationResult.Payload;
        }

        return (true, null);
    }
    
    private BackchannelAuthenticationRequestValidationResult Invalid(string error, string? errorDescription = null)
    {
        return new BackchannelAuthenticationRequestValidationResult(validatedRequest, error, errorDescription);
    }

    private void LogError(string? message = null, object? values = null)
    {
        LogWithRequestDetails(LogLevel.Error, message, values);
    }

    private void LogWithRequestDetails(LogLevel logLevel, string? message = null, object? values = null)
    {
        /*var details = new BackchannelAuthenticationRequestValidationLog(validatedRequest, options.Logging.BackchannelAuthenticationRequestSensitiveValuesFilter);

        if (message.IsPresent())
        {
            try
            {
                if (values == null)
                {
                    _logger.Log(logLevel, message + ", {@details}", details);
                }
                else
                {
                    _logger.Log(logLevel, message + "{@values}, details: {@details}", values, details);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Error logging {exception}, request details: {@details}", ex.Message, details);
            }
        }
        else
        {
            _logger.Log(logLevel, "{@details}", details);
        }*/
    }

    private void LogSuccess()
    {
        LogWithRequestDetails(LogLevel.Information, "Backchannel authentication request validation success");
    }
}