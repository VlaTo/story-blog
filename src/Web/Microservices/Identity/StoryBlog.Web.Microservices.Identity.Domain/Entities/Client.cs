using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

public class Client : IEntity, IHasId<int>
{
    public int Id
    {
        get;
        set;
    }

    public bool Enabled
    {
        get;
        set;
    }

    public string ClientId
    {
        get;
        set;
    }

    public string ProtocolType
    {
        get;
        set;
    }

    public List<ClientSecret> ClientSecrets
    {
        get;
        set;
    }

    public bool RequireClientSecret
    {
        get;
        set;
    }

    public string ClientName
    {
        get;
        set;
    }

    public string? Description
    {
        get;
        set;
    }

    public string ClientUri
    {
        get;
        set;
    }

    public string? LogoUri
    {
        get;
        set;
    }

    public bool RequireConsent
    {
        get;
        set;
    }

    public bool AllowRememberConsent
    {
        get;
        set;
    }

    public bool AlwaysIncludeUserClaimsInIdToken
    {
        get;
        set;
    }

    public List<ClientGrantType> AllowedGrantTypes
    {
        get;
        set;
    }

    public bool RequirePkce
    {
        get;
        set;
    }

    public bool AllowPlainTextPkce
    {
        get;
        set;
    }

    public bool RequireRequestObject
    {
        get;
        set;
    }

    public bool AllowAccessTokensViaBrowser
    {
        get;
        set;
    }

    public List<ClientRedirectUri> RedirectUris
    {
        get;
        set;
    }

    public List<ClientPostSignOutRedirectUri> PostLogoutRedirectUris
    {
        get;
        set;
    }

    public string FrontChannelLogoutUri
    {
        get;
        set;
    }

    public bool FrontChannelLogoutSessionRequired
    {
        get;
        set;
    }

    public string BackChannelLogoutUri
    {
        get;
        set;
    }

    public bool BackChannelLogoutSessionRequired
    {
        get;
        set;
    }

    public bool AllowOfflineAccess
    {
        get;
        set;
    }

    public List<ClientScope> AllowedScopes
    {
        get;
        set;
    }

    public TimeSpan IdentityTokenLifetime
    {
        get;
        set;
    }

    public string AllowedIdentityTokenSigningAlgorithms
    {
        get;
        set;
    }

    public TimeSpan AccessTokenLifetime
    {
        get;
        set;
    }

    public TimeSpan AuthorizationCodeLifetime
    {
        get;
        set;
    }

    public int? ConsentLifetime
    {
        get;
        set;
    }

    public TimeSpan AbsoluteRefreshTokenLifetime
    {
        get;
        set;
    }

    public TimeSpan SlidingRefreshTokenLifetime
    {
        get;
        set;
    }

    public TokenUsage RefreshTokenUsage
    {
        get;
        set;
    }

    public bool UpdateAccessTokenClaimsOnRefresh
    {
        get;
        set;
    }

    public TokenExpiration RefreshTokenExpiration
    {
        get;
        set;
    }

    public AccessTokenType AccessTokenType
    {
        get;
        set;
    }

    public bool EnableLocalLogin
    {
        get;
        set;
    }

    public List<ClientIdPRestriction> IdentityProviderRestrictions
    {
        get;
        set;
    }

    public bool IncludeJwtId
    {
        get;
        set;
    }

    public List<ClientClaim> Claims
    {
        get;
        set;
    }

    public bool AlwaysSendClientClaims
    {
        get;
        set;
    }

    public string ClientClaimsPrefix
    {
        get;
        set;
    }

    public string PairWiseSubjectSalt
    {
        get;
        set;
    }

    public List<ClientCorsOrigin> AllowedCorsOrigins
    {
        get;
        set;
    }

    public List<ClientProperty> Properties
    {
        get;
        set;
    }

    public TimeSpan? UserSsoLifetime
    {
        get;
        set;
    }

    public string UserCodeType
    {
        get;
        set;
    }

    public TimeSpan DeviceCodeLifetime
    {
        get;
        set;
    }

    public TimeSpan? CibaLifetime
    {
        get;
        set;
    }

    public TimeSpan? PollingInterval
    {
        get;
        set;
    }

    public DateTimeOffset Created
    {
        get;
        set;
    }

    public DateTimeOffset? Updated
    {
        get;
        set;
    }

    public DateTimeOffset? LastAccessed
    {
        get;
        set;
    }

    public bool NonEditable
    {
        get;
        set;
    }
}