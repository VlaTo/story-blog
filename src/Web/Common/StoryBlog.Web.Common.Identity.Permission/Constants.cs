namespace StoryBlog.Web.Common.Identity.Permission;

public static class Constants
{



    public enum ScopeRequirement
    {
        None,
        ResourceOnly,
        IdentityOnly,
        Identity
    }



    public static string[] SupportedSubjectTypes =
    {
        "pairwise", "public"
    };



    public static class KnownAcrValues
    {
        public const string HomeRealm = "idp:";
        public const string Tenant = "tenant:";

        public static readonly string[] All = [HomeRealm, Tenant];
    }



    public static class UIConstants
    {
        // the limit after which old messages are purged
        public const int CookieMessageThreshold = 2;

        public static class DefaultRoutePathParams
        {
            public const string Error = "errorId";
            public const string Login = "returnUrl";
            public const string Consent = "returnUrl";
            public const string Logout = "logoutId";
            public const string EndSessionCallback = "endSessionId";
            public const string Custom = "returnUrl";
            public const string UserCode = "userCode";
        }

        public static class DefaultRoutePaths
        {
            public const string Login = "/account/login";
            public const string Logout = "/account/logout";
            public const string Consent = "/consent";
            public const string Error = "/home/error";
            public const string DeviceVerification = "/device";
        }
    }

    public static class EndpointNames
    {
        public const string Authorize = "Authorize";
        public const string BackchannelAuthentication = "BackchannelAuthentication";
        public const string Token = "Token";
        public const string DeviceAuthorization = "DeviceAuthorization";
        public const string Discovery = "Discovery";
        public const string Introspection = "Introspection";
        public const string Revocation = "Revocation";
        public const string EndSession = "Endsession";
        public const string CheckSession = "Checksession";
        public const string UserInfo = "Userinfo";
    }

    public static class ProtocolRoutePaths
    {
        public const string ConnectPathPrefix = "connect";

        public const string Authorize = ConnectPathPrefix + "/authorize";
        public const string AuthorizeCallback = Authorize + "/callback";
        public const string DiscoveryConfiguration = ".well-known/openid-configuration";
        public const string DiscoveryWebKeys = DiscoveryConfiguration + "/jwks";
        public const string BackchannelAuthentication = ConnectPathPrefix + "/ciba";
        public const string Token = ConnectPathPrefix + "/token";
        public const string Revocation = ConnectPathPrefix + "/revocation";
        public const string UserInfo = ConnectPathPrefix + "/userinfo";
        public const string Introspection = ConnectPathPrefix + "/introspect";
        public const string EndSession = ConnectPathPrefix + "/endsession";
        public const string EndSessionCallback = EndSession + "/callback";
        public const string CheckSession = ConnectPathPrefix + "/checksession";
        public const string DeviceAuthorization = ConnectPathPrefix + "/deviceauthorization";

        public const string MtlsPathPrefix = ConnectPathPrefix + "/mtls";
        public const string MtlsToken = MtlsPathPrefix + "/token";
        public const string MtlsRevocation = MtlsPathPrefix + "/revocation";
        public const string MtlsIntrospection = MtlsPathPrefix + "/introspect";
        public const string MtlsDeviceAuthorization = MtlsPathPrefix + "/deviceauthorization";

        public static readonly string[] CorsPaths =
        {
            DiscoveryConfiguration,
            DiscoveryWebKeys,
            Token,
            UserInfo,
            Revocation
        };
    }

    public static class EnvironmentKeys
    {
        public const string IdentityServerBasePath = "idsvr:IdentityServerBasePath";
        [Obsolete("The IdentityServerOrigin constant is obsolete.")]
        public const string IdentityServerOrigin = "idsvr:IdentityServerOrigin"; // todo: deprecate
        public const string SignOutCalled = "idsvr:IdentityServerSignOutCalled";
    }

    public static class TokenTypeHints
    {
        public const string RefreshToken = "refresh_token";
        public const string AccessToken = "access_token";
    }

    public static List<string> SupportedTokenTypeHints = new List<string>
    {
        TokenTypeHints.RefreshToken,
        TokenTypeHints.AccessToken
    };

    public static class RevocationErrors
    {
        public const string UnsupportedTokenType = "unsupported_token_type";
    }

    public class Filters
    {
        // filter for claims from an incoming access token (e.g. used at the user profile endpoint)
        public static readonly string[] ProtocolClaimsFilter = {
            JwtClaimTypes.AccessTokenHash,
            JwtClaimTypes.Audience,
            JwtClaimTypes.AuthorizedParty,
            JwtClaimTypes.AuthorizationCodeHash,
            JwtClaimTypes.ClientId,
            JwtClaimTypes.Expiration,
            JwtClaimTypes.IssuedAt,
            JwtClaimTypes.Issuer,
            JwtClaimTypes.JwtId,
            JwtClaimTypes.Nonce,
            JwtClaimTypes.NotBefore,
            JwtClaimTypes.ReferenceTokenId,
            JwtClaimTypes.SessionId,
            JwtClaimTypes.Scope
        };

        // filter list for claims returned from profile service prior to creating tokens
        public static readonly string[] ClaimsServiceFilterClaimTypes = {
            // TODO: consider JwtClaimTypes.AuthenticationContextClassReference,
            JwtClaimTypes.AccessTokenHash,
            JwtClaimTypes.Audience,
            JwtClaimTypes.AuthenticationMethod,
            JwtClaimTypes.AuthenticationTime,
            JwtClaimTypes.AuthorizedParty,
            JwtClaimTypes.AuthorizationCodeHash,
            JwtClaimTypes.ClientId,
            JwtClaimTypes.Expiration,
            JwtClaimTypes.IdentityProvider,
            JwtClaimTypes.IssuedAt,
            JwtClaimTypes.Issuer,
            JwtClaimTypes.JwtId,
            JwtClaimTypes.Nonce,
            JwtClaimTypes.NotBefore,
            JwtClaimTypes.ReferenceTokenId,
            JwtClaimTypes.SessionId,
            JwtClaimTypes.Subject,
            JwtClaimTypes.Scope,
            JwtClaimTypes.Confirmation
        };

        public static readonly string[] JwtRequestClaimTypesFilter = {
            JwtClaimTypes.Audience,
            JwtClaimTypes.Expiration,
            JwtClaimTypes.IssuedAt,
            JwtClaimTypes.Issuer,
            JwtClaimTypes.NotBefore,
            JwtClaimTypes.JwtId
        };
    }

    public static class WsFedSignOut
    {
        public const string LogoutUriParameterName = "wa";
        public const string LogoutUriParameterValue = "wsignoutcleanup1.0";
    }

    public static class AuthorizationParamsStore
    {
        public const string MessageStoreIdParameterName = "authzId";
    }

    public static class CurveOids
    {
        public const string P256 = "1.2.840.10045.3.1.7";
        public const string P384 = "1.3.132.0.34";
        public const string P521 = "1.3.132.0.35";
    }
}