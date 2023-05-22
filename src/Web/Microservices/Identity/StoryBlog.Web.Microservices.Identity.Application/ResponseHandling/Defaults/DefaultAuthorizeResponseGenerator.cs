using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Microservices.Identity.Application.Authorization.Responses;
using StoryBlog.Web.Microservices.Identity.Application.Configuration;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Application.ResponseHandling.Generators;
using StoryBlog.Web.Microservices.Identity.Application.Services;
using StoryBlog.Web.Microservices.Identity.Application.Storage;
using StoryBlog.Web.Microservices.Identity.Application.Stores;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Requests;

namespace StoryBlog.Web.Microservices.Identity.Application.ResponseHandling.Defaults;

/// <summary>
/// The authorize response generator
/// </summary>
/// <seealso cref="IAuthorizeResponseGenerator" />
public class DefaultAuthorizeResponseGenerator : IAuthorizeResponseGenerator
{
    /// <summary>
    /// The options
    /// </summary>
    protected IdentityServerOptions Options;

    /// <summary>
    /// The token service
    /// </summary>
    protected readonly ITokenService TokenService;

    /// <summary>
    /// The authorization code store
    /// </summary>
    protected readonly IAuthorizationCodeStore AuthorizationCodeStore;

    /// <summary>
    /// The event service
    /// </summary>
    protected readonly IEventService Events;

    /// <summary>
    /// The logger
    /// </summary>
    protected readonly ILogger Logger;

    /// <summary>
    /// The clock
    /// </summary>
    protected readonly ISystemClock Clock;

    /// <summary>
    /// The key material service
    /// </summary>
    protected readonly IKeyMaterialService KeyMaterialService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizeResponseGenerator"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <param name="clock">The clock.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="tokenService">The token service.</param>
    /// <param name="keyMaterialService"></param>
    /// <param name="authorizationCodeStore">The authorization code store.</param>
    /// <param name="events">The events.</param>
    public DefaultAuthorizeResponseGenerator(
        IdentityServerOptions options,
        ISystemClock clock,
        ITokenService tokenService,
        IKeyMaterialService keyMaterialService,
        IAuthorizationCodeStore authorizationCodeStore,
        ILogger<DefaultAuthorizeResponseGenerator> logger,
        IEventService events)
    {
        Options = options;
        Clock = clock;
        TokenService = tokenService;
        KeyMaterialService = keyMaterialService;
        AuthorizationCodeStore = authorizationCodeStore;
        Events = events;
        Logger = logger;
    }

    /// <summary>
    /// Creates the response
    /// </summary>
    /// <param name="request">The request.</param>
    /// <returns></returns>
    /// <exception cref="System.InvalidOperationException">invalid grant type: " + request.GrantType</exception>
    public virtual async Task<AuthorizeResponse> CreateResponseAsync(ValidatedAuthorizeRequest request)
    {
        using var activity = Tracing.BasicActivitySource.StartActivity("AuthorizeResponseGenerator.CreateResponse");

        if (GrantType.AuthorizationCode == request.GrantType)
        {
            return await CreateCodeFlowResponseAsync(request);
        }

        /*if (GrantType.Implicit == request.GrantType)
        {
            return await CreateImplicitFlowResponseAsync(request);
        }

        if (GrantType.Hybrid == request.GrantType)
        {
            return await CreateHybridFlowResponseAsync(request);
        }*/

        Logger.LogError("Unsupported grant type: " + request.GrantType);

        throw new InvalidOperationException("invalid grant type: " + request.GrantType);
    }

    /// <summary>
    /// Creates the response for a code flow request
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    protected virtual async Task<AuthorizeResponse> CreateCodeFlowResponseAsync(ValidatedAuthorizeRequest request)
    {
        Logger.LogDebug("Creating Authorization Code Flow response.");

        var code = await CreateCodeAsync(request);
        var id = await AuthorizationCodeStore.StoreAuthorizationCodeAsync(code);

        var response = new AuthorizeResponse
        {
            Issuer = request.IssuerName,
            Request = request,
            Code = id,
            SessionState = request.GenerateSessionStateValue()
        };

        return response;
    }

    /// <summary>
    /// Creates an authorization code
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    protected virtual async Task<AuthorizationCode> CreateCodeAsync(ValidatedAuthorizeRequest request)
    {
        string? stateHash = null;

        if (Options.EmitStateHash && request.State.IsPresent())
        {
            var credential = await KeyMaterialService.GetSigningCredentialsAsync(request.Client.AllowedIdentityTokenSigningAlgorithms);

            if (null == credential)
            {
                throw new InvalidOperationException("No signing credential is configured.");
            }

            var algorithm = credential.Algorithm;

            stateHash = CryptoHelper.CreateHashClaimValue(request.State, algorithm);
        }

        var code = new AuthorizationCode
        {
            CreationTime = Clock.UtcNow.UtcDateTime,
            ClientId = request.Client.ClientId,
            Lifetime = request.Client.AuthorizationCodeLifetime,
            Subject = request.Subject,
            SessionId = request.SessionId,
            Description = request.Description,
            CodeChallenge = request.CodeChallenge.Sha256(),
            CodeChallengeMethod = request.CodeChallengeMethod,

            IsOpenId = request.IsOpenIdRequest,
            RequestedScopes = request.ValidatedResources.RawScopeValues,
            RequestedResourceIndicators = request.RequestedResourceIndiators,
            RedirectUri = request.RedirectUri,
            Nonce = request.Nonce,
            StateHash = stateHash,

            WasConsentShown = request.WasConsentShown
        };

        return code;
    }
}