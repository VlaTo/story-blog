using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace StoryBlog.Web.Identity.Client.Responses;

public class DiscoveryDocumentResponse : ProtocolResponse
{
    public DiscoveryPolicy Policy
    {
        get; 
        set;
    }

    public JsonWebKeySet? KeySet
    {
        get; 
        set;
    }

    /*public MtlsEndpointAliases? MtlsEndpointAliases
    {
        get; 
        internal set;
    }*/

    public string? Issuer => GetStringOrDefault(OidcConstants.Discovery.Issuer);
    
    public string? AuthorizeEndpoint => GetStringOrDefault(OidcConstants.Discovery.AuthorizationEndpoint);
    
    public string? TokenEndpoint => GetStringOrDefault(OidcConstants.Discovery.TokenEndpoint);
    
    public string? UserInfoEndpoint => GetStringOrDefault(OidcConstants.Discovery.UserInfoEndpoint);
    
    public string? IntrospectionEndpoint => GetStringOrDefault(OidcConstants.Discovery.IntrospectionEndpoint);
    
    public string? RevocationEndpoint => GetStringOrDefault(OidcConstants.Discovery.RevocationEndpoint);
    
    public string? DeviceAuthorizationEndpoint => GetStringOrDefault(OidcConstants.Discovery.DeviceAuthorizationEndpoint);
    
    public string? BackchannelAuthenticationEndpoint => GetStringOrDefault(OidcConstants.Discovery.BackchannelAuthenticationEndpoint);

    public string? JwksUri => GetStringOrDefault(OidcConstants.Discovery.JwksUri);

    public string? EndSessionEndpoint => GetStringOrDefault(OidcConstants.Discovery.EndSessionEndpoint);

    public string? CheckSessionIframe => GetStringOrDefault(OidcConstants.Discovery.CheckSessionIframe);

    public string? RegistrationEndpoint => GetStringOrDefault(OidcConstants.Discovery.RegistrationEndpoint);

    public string? PushedAuthorizationRequestEndpoint => GetStringOrDefault(OidcConstants.Discovery.PushedAuthorizationRequestEndpoint);

    public bool? FrontChannelLogoutSupported => GetBooleanOrDefault(OidcConstants.Discovery.FrontChannelLogoutSupported);

    public bool? FrontChannelLogoutSessionSupported => GetBooleanOrDefault(OidcConstants.Discovery.FrontChannelLogoutSessionSupported);

    public IEnumerable<string> GrantTypesSupported => GetStringArray(OidcConstants.Discovery.GrantTypesSupported);

    public IEnumerable<string> CodeChallengeMethodsSupported => GetStringArray(OidcConstants.Discovery.CodeChallengeMethodsSupported);

    public IEnumerable<string> ScopesSupported => GetStringArray(OidcConstants.Discovery.ScopesSupported);

    public IEnumerable<string> SubjectTypesSupported => GetStringArray(OidcConstants.Discovery.SubjectTypesSupported);

    public IEnumerable<string> ResponseModesSupported => GetStringArray(OidcConstants.Discovery.ResponseModesSupported);

    public IEnumerable<string> ResponseTypesSupported => GetStringArray(OidcConstants.Discovery.ResponseTypesSupported);

    public IEnumerable<string> ClaimsSupported => GetStringArray(OidcConstants.Discovery.ClaimsSupported);

    public IEnumerable<string> TokenEndpointAuthenticationMethodsSupported => GetStringArray(OidcConstants.Discovery.TokenEndpointAuthenticationMethodsSupported);

    public IEnumerable<string> BackchannelTokenDeliveryModesSupported => GetStringArray(OidcConstants.Discovery.BackchannelTokenDeliveryModesSupported);

    public bool? BackchannelUserCodeParameterSupported => GetBooleanOrDefault(OidcConstants.Discovery.BackchannelUserCodeParameterSupported);

    public bool? RequirePushedAuthorizationRequests => GetBooleanOrDefault(OidcConstants.Discovery.RequirePushedAuthorizationRequests);

    public bool? GetBooleanOrDefault(string name) => GetPropertyOrDefault(name)?.GetBoolean();

    public IEnumerable<string> GetStringArray(string name)
    {
        var element = GetPropertyOrDefault(name);

        if (element.HasValue)
        {
            return element.Value.EnumerateArray()
                .Select(x => x.GetString())
                .Where(x => false == String.IsNullOrEmpty(x))
                .Select(x => x!);
        }

        return Array.Empty<string>();
    }

    public string ValidateEndpoints(JsonElement? json, DiscoveryPolicy policy)
    {
        if (null == json)
        {
            throw new ArgumentNullException(nameof(json));
        }

        // allowed hosts
        var allowedHosts = new HashSet<string>(policy.AdditionalEndpointBaseAddresses.Select(e => new Uri(e).Authority))
        {
            new Uri(policy.Authority).Authority
        };

        // allowed authorities (hosts + base address)
        var allowedAuthorities = new HashSet<string>(policy.AdditionalEndpointBaseAddresses)
        {
            policy.Authority
        };

        // Can't actually be null, because we check that and throw at the beginning of the method
        foreach (var element in json.Value.EnumerateObject())
        {
            if (element.Name.EndsWith("endpoint", StringComparison.OrdinalIgnoreCase) ||
                element.Name.Equals(OidcConstants.Discovery.JwksUri, StringComparison.OrdinalIgnoreCase) ||
                element.Name.Equals(OidcConstants.Discovery.CheckSessionIframe, StringComparison.OrdinalIgnoreCase))
            {
                var endpoint = element.Value.ToString();
                var isValidUri = Uri.TryCreate(endpoint, UriKind.Absolute, out var uri);

                if (!isValidUri)
                {
                    return $"Malformed endpoint: {endpoint}";
                }

                if (!DiscoveryEndpoint.IsValidScheme(uri!))
                {
                    return $"Malformed endpoint: {endpoint}";
                }

                if (!DiscoveryEndpoint.IsSecureScheme(uri!, policy))
                {
                    return $"Endpoint does not use HTTPS: {endpoint}";
                }

                if (policy.ValidateEndpoints)
                {
                    // if endpoint is on exclude list, don't validate
                    if (policy.EndpointValidationExcludeList.Contains(element.Name))
                    {
                        continue;
                    }

                    if (false == allowedHosts.Contains(uri!.Authority))
                    {
                        return $"Endpoint is on a different host than authority: {endpoint}";
                    }

                    /*var isAllowed = false;

                    foreach (var host in allowedHosts)
                    {
                        if (String.Equals(host, uri!.Authority))
                        {
                            isAllowed = true;
                        }
                    }

                    if (!isAllowed)
                    {
                        return $"Endpoint is on a different host than authority: {endpoint}";
                    }*/

                    var strategy = policy.AuthorityValidationStrategy ?? DiscoveryPolicy.DefaultAuthorityValidationStrategy;
                    var endpointValidationResult = strategy.IsEndpointValid(endpoint, allowedAuthorities);
                    
                    if (!endpointValidationResult.Success)
                    {
                        return endpointValidationResult.ErrorMessage;
                    }
                }
            }
        }

        if (policy.RequireKeySet)
        {
            if (String.IsNullOrWhiteSpace(JwksUri))
            {
                return "Keyset is missing";
            }
        }

        return String.Empty;
    }
    
    protected override Task InitializeAsync(object? initializationData = null)
    {
        if (HttpResponse?.IsSuccessStatusCode != true)
        {
            ErrorMessage = initializationData as string;
            return Task.CompletedTask;
        }

        Policy = initializationData as DiscoveryPolicy ?? new DiscoveryPolicy();

        var validationError = Validate(Policy);

        if (false == String.IsNullOrEmpty(validationError))
        {
            Json = default;
            ErrorType = ResponseErrorType.PolicyViolation;
            ErrorMessage = validationError;
        }

        //MtlsEndpointAliases =
        //    new MtlsEndpointAliases(Json?.TryGetValue(OidcConstants.Discovery.MtlsEndpointAliases));

        return Task.CompletedTask;
    }

    private string Validate(DiscoveryPolicy policy)
    {
        if (policy.ValidateIssuerName)
        {
            var strategy = policy.AuthorityValidationStrategy;
            var issuerValidationResult = strategy.IsIssuerNameValid(Issuer!, policy.Authority);

            if (false == issuerValidationResult.Success)
            {
                return issuerValidationResult.ErrorMessage;
            }
        }

        var error = ValidateEndpoints(Json, policy);

        return String.IsNullOrEmpty(error) ? String.Empty : error;
    }
}