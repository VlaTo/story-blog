using Microsoft.IdentityModel.Tokens;
using StoryBlog.Web.Common.Identity.Permission;
using StoryBlog.Web.Identity;
using static StoryBlog.Web.Common.Identity.Permission.Constants;

namespace StoryBlog.Web.Microservices.Identity.Application;

public class IdentityServerConstants
{
    public const string LocalIdentityProvider = "local";
    public const string DefaultCookieAuthenticationScheme = "sbid";
    public const string SignOutScheme = "idsrv";
    public const string ExternalCookieAuthenticationScheme = "sbid.external";
    public const string DefaultCheckSessionCookieName = "sbid.session";
    public const string AccessTokenAudience = "{0}resources";
    public const string JwtRequestClientKey = "sbid.jwtrequesturi.client";
    public const string IdentityServerName = "SampleBlog.IdentityServer";
    public const string IdentityServerAuthenticationType = IdentityServerName;
    public const string ExternalAuthenticationMethod = "external";
    public const string DefaultHashAlgorithm = "SHA256";
    public static readonly TimeSpan DefaultCookieTimeSpan = TimeSpan.FromHours(10);
    public static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromMinutes(60);
    
    public const string SuppressedPrompt = "suppressed_" + OidcConstants.AuthorizeRequest.Prompt;

    public static readonly List<string> SupportedResponseTypes =
    [
        OidcConstants.ResponseTypes.Code,
        OidcConstants.ResponseTypes.Token,
        OidcConstants.ResponseTypes.IdToken,
        OidcConstants.ResponseTypes.IdTokenToken,
        OidcConstants.ResponseTypes.CodeIdToken,
        OidcConstants.ResponseTypes.CodeToken,
        OidcConstants.ResponseTypes.CodeIdTokenToken
    ];
    
    public static readonly Dictionary<string, string> ResponseTypeToGrantTypeMapping = new()
    {
        { OidcConstants.ResponseTypes.Code, GrantType.AuthorizationCode },
        { OidcConstants.ResponseTypes.Token, GrantType.Implicit },
        { OidcConstants.ResponseTypes.IdToken, GrantType.Implicit },
        { OidcConstants.ResponseTypes.IdTokenToken, GrantType.Implicit },
        { OidcConstants.ResponseTypes.CodeIdToken, GrantType.Hybrid },
        { OidcConstants.ResponseTypes.CodeToken, GrantType.Hybrid },
        { OidcConstants.ResponseTypes.CodeIdTokenToken, GrantType.Hybrid }
    };
    
    public static readonly List<string> SupportedCodeChallengeMethods =
    [
        OidcConstants.CodeChallengeMethods.Plain,
        OidcConstants.CodeChallengeMethods.Sha256
    ];

    public static readonly List<string> AllowedGrantTypesForAuthorizeEndpoint =
    [
        GrantType.AuthorizationCode,
        GrantType.Implicit,
        GrantType.Hybrid
    ];

    /// <summary>
    /// Constants for local IdentityServer access token authentication.
    /// </summary>
    public static class LocalApi
    {
        /// <summary>
        /// The authentication scheme when using the AddLocalApi helper.
        /// </summary>
        public const string AuthenticationScheme = "IdentityServerAccessToken";

        /// <summary>
        /// The API scope name when using the AddLocalApiAuthentication helper.
        /// </summary>
        public const string ScopeName = "IdentityServerApi";

        /// <summary>
        /// The authorization policy name when using the AddLocalApiAuthentication helper.
        /// </summary>
        public const string PolicyName = AuthenticationScheme;
    }

    public static class ProtocolTypes
    {
        public const string OpenIdConnect = "oidc";
        public const string WsFederation = "wsfed";
        public const string Saml2p = "saml2p";
    }

    public static class TokenTypes
    {
        public const string IdentityToken = "id_token";
        public const string AccessToken = "access_token";
    }

    public static class ClaimValueTypes
    {
        public const string Json = "json";
    }
    
    public static readonly Dictionary<string, IEnumerable<string>> AllowedResponseModesForGrantType = new()
    {
        { GrantType.AuthorizationCode, new[] { OidcConstants.ResponseModes.Query, OidcConstants.ResponseModes.FormPost, OidcConstants.ResponseModes.Fragment } },
        { GrantType.Hybrid, new[] { OidcConstants.ResponseModes.Fragment, OidcConstants.ResponseModes.FormPost }},
        { GrantType.Implicit, new[] { OidcConstants.ResponseModes.Fragment, OidcConstants.ResponseModes.FormPost }}
    };

    public static readonly Dictionary<string, ScopeRequirement> ResponseTypeToScopeRequirement = new()
    {
        { OidcConstants.ResponseTypes.Code, ScopeRequirement.None },
        { OidcConstants.ResponseTypes.Token, ScopeRequirement.ResourceOnly },
        { OidcConstants.ResponseTypes.IdToken, ScopeRequirement.IdentityOnly },
        { OidcConstants.ResponseTypes.IdTokenToken, ScopeRequirement.Identity },
        { OidcConstants.ResponseTypes.CodeIdToken, ScopeRequirement.Identity },
        { OidcConstants.ResponseTypes.CodeToken, ScopeRequirement.Identity },
        { OidcConstants.ResponseTypes.CodeIdTokenToken, ScopeRequirement.Identity }
    };

    public static readonly List<string> SupportedResponseModes =
    [
        OidcConstants.ResponseModes.FormPost,
        OidcConstants.ResponseModes.Query,
        OidcConstants.ResponseModes.Fragment
    ];

    public static readonly List<string> SupportedDisplayModes =
    [
        OidcConstants.DisplayModes.Page,
        OidcConstants.DisplayModes.Popup,
        OidcConstants.DisplayModes.Touch,
        OidcConstants.DisplayModes.Wap
    ];

    public static readonly List<string> SupportedPromptModes =
    [
        OidcConstants.PromptModes.None,
        OidcConstants.PromptModes.Login,
        OidcConstants.PromptModes.Consent,
        OidcConstants.PromptModes.SelectAccount
    ];

    public static readonly Dictionary<string, IEnumerable<string>> ScopeToClaimsMapping = new()
    {
        { OidcConstants.StandardScopes.Profile, new[]
        {
            JwtClaimTypes.Name,
            JwtClaimTypes.FamilyName,
            JwtClaimTypes.GivenName,
            JwtClaimTypes.MiddleName,
            JwtClaimTypes.NickName,
            JwtClaimTypes.PreferredUserName,
            JwtClaimTypes.Profile,
            JwtClaimTypes.Picture,
            JwtClaimTypes.WebSite,
            JwtClaimTypes.Gender,
            JwtClaimTypes.BirthDate,
            JwtClaimTypes.ZoneInfo,
            JwtClaimTypes.Locale,
            JwtClaimTypes.UpdatedAt
        }},
        { OidcConstants.StandardScopes.Email, new[]
        {
            JwtClaimTypes.Email,
            JwtClaimTypes.EmailVerified
        }},
        { OidcConstants.StandardScopes.Address, new[]
        {
            JwtClaimTypes.Address
        }},
        { OidcConstants.StandardScopes.Phone, new[]
        {
            JwtClaimTypes.PhoneNumber,
            JwtClaimTypes.PhoneNumberVerified
        }},
        { OidcConstants.StandardScopes.OpenId, new[]
        {
            JwtClaimTypes.Subject
        }}
    };

    public static class SigningAlgorithms
    {
        public const string SHA_256 = "RS256";
        public const string RSA_SHA_256 = "RS256";
    }

    public static class ParsedSecretTypes
    {
        public const string NoSecret = "NoSecret";
        public const string SharedSecret = "SharedSecret";
        public const string X509Certificate = "X509Certificate";
        public const string JwtBearer = "urn:ietf:params:oauth:client-assertion-type:jwt-bearer";
    }

    public static class SecretTypes
    {
        public const string SharedSecret = "SharedSecret";
        public const string X509CertificateThumbprint = "X509Thumbprint";
        public const string X509CertificateName = "X509Name";
        public const string X509CertificateBase64 = "X509CertificateBase64";
        public const string JsonWebKey = "JWK";
    }

    public static class ProfileDataCallers
    {
        public const string UserInfoEndpoint = "UserInfoEndpoint";
        public const string ClaimsProviderIdentityToken = "ClaimsProviderIdentityToken";
        public const string ClaimsProviderAccessToken = "ClaimsProviderAccessToken";
    }

    public static class ProfileIsActiveCallers
    {
        public const string AuthorizeEndpoint = "AuthorizeEndpoint";
        public const string IdentityTokenValidation = "IdentityTokenValidation";
        public const string AccessTokenValidation = "AccessTokenValidation";
        public const string ResourceOwnerValidation = "ResourceOwnerValidation";
        public const string ExtensionGrantValidation = "ExtensionGrantValidation";
        public const string RefreshTokenValidation = "RefreshTokenValidation";
        public const string AuthorizationCodeValidation = "AuthorizationCodeValidation";
        public const string UserInfoRequestValidation = "UserInfoRequestValidation";
        public const string DeviceCodeValidation = "DeviceCodeValidation";

        public const string BackchannelAuthenticationRequestIdValidation = "BackchannelAuthenticationRequestIdValidation";
    }

    public static readonly IEnumerable<string> SupportedSigningAlgorithms =
    [
        SecurityAlgorithms.RsaSha256,
        SecurityAlgorithms.RsaSha384,
        SecurityAlgorithms.RsaSha512,

        SecurityAlgorithms.RsaSsaPssSha256,
        SecurityAlgorithms.RsaSsaPssSha384,
        SecurityAlgorithms.RsaSsaPssSha512,

        SecurityAlgorithms.EcdsaSha256,
        SecurityAlgorithms.EcdsaSha384,
        SecurityAlgorithms.EcdsaSha512
    ];

    public static Dictionary<string, int> ProtectedResourceErrorStatusCodes = new()
    {
        { OidcConstants.ProtectedResourceErrors.InvalidToken,      401 },
        { OidcConstants.ProtectedResourceErrors.ExpiredToken,      401 },
        { OidcConstants.ProtectedResourceErrors.InvalidRequest,    400 },
        { OidcConstants.ProtectedResourceErrors.InsufficientScope, 403 }
    };

    public enum RsaSigningAlgorithm
    {
        RS256,
        RS384,
        RS512,

        PS256,
        PS384,
        PS512
    }

    public enum ECDsaSigningAlgorithm
    {
        ES256,
        ES384,
        ES512
    }

    public static class PersistedGrantTypes
    {
        public const string AuthorizationCode = "authorization_code";
        public const string BackChannelAuthenticationRequest = "ciba";
        public const string ReferenceToken = "reference_token";
        public const string RefreshToken = "refresh_token";
        public const string UserConsent = "user_consent";
        public const string DeviceCode = "device_code";
        public const string UserCode = "user_code";
    }

    public static class UserCodeTypes
    {
        public const string Numeric = "Numeric";
    }

    public static class HttpClients
    {
        public const int DefaultTimeoutSeconds = 10;
        public const string JwtRequestUriHttpClient = "IdentityServer:JwtRequestUriClient";
        public const string BackChannelLogoutHttpClient = "IdentityServer:BackChannelLogoutClient";
    }

    public static class ClaimTypes
    {
        public const string Tenant = "tenant";
    }
}