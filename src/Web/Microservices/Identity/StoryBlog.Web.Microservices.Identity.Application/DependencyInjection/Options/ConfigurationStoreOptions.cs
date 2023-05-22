using Microsoft.EntityFrameworkCore;
using StoryBlog.Web.Microservices.Identity.Domain;

namespace StoryBlog.Web.Microservices.Identity.Application.DependencyInjection.Options;

/// <summary>
/// Options for configuring the configuration context.
/// </summary>
public class ConfigurationStoreOptions
{
    /// <summary>
    /// Callback to configure the EF DbContext.
    /// </summary>
    /// <value>
    /// The configure database context.
    /// </value>
    public Action<DbContextOptionsBuilder>? ConfigureDbContext
    {
        get;
        set;
    }

    /// <summary>
    /// Callback in DI resolve the EF DbContextOptions. If set, ConfigureDbContext will not be used.
    /// </summary>
    /// <value>
    /// The configure database context.
    /// </value>
    public Action<IServiceProvider, DbContextOptionsBuilder>? ResolveDbContextOptions
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the default schema.
    /// </summary>
    /// <value>
    /// The default schema.
    /// </value>
    public string? DefaultSchema
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the identity resource table configuration.
    /// </summary>
    /// <value>
    /// The identity resource.
    /// </value>
    public TableConfiguration IdentityResource
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the identity claim table configuration.
    /// </summary>
    /// <value>
    /// The identity claim.
    /// </value>
    public TableConfiguration IdentityResourceClaim
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the identity resource property table configuration.
    /// </summary>
    /// <value>
    /// The client property.
    /// </value>
    public TableConfiguration IdentityResourceProperty
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the API resource table configuration.
    /// </summary>
    /// <value>
    /// The API resource.
    /// </value>
    public TableConfiguration ApiResource
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the API secret table configuration.
    /// </summary>
    /// <value>
    /// The API secret.
    /// </value>
    public TableConfiguration ApiResourceSecret
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the API scope table configuration.
    /// </summary>
    /// <value>
    /// The API scope.
    /// </value>
    public TableConfiguration ApiResourceScope
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the API claim table configuration.
    /// </summary>
    /// <value>
    /// The API claim.
    /// </value>
    public TableConfiguration ApiResourceClaim
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the API resource property table configuration.
    /// </summary>
    /// <value>
    /// The client property.
    /// </value>
    public TableConfiguration ApiResourceProperty
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the client table configuration.
    /// </summary>
    /// <value>
    /// The client.
    /// </value>
    public TableConfiguration Client
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the type of the client grant table configuration.
    /// </summary>
    /// <value>
    /// The type of the client grant.
    /// </value>
    public TableConfiguration ClientGrantType
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the client redirect URI table configuration.
    /// </summary>
    /// <value>
    /// The client redirect URI.
    /// </value>
    public TableConfiguration ClientRedirectUri
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the client post logout redirect URI table configuration.
    /// </summary>
    /// <value>
    /// The client post logout redirect URI.
    /// </value>
    public TableConfiguration ClientPostLogoutRedirectUri
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the client scopes table configuration.
    /// </summary>
    /// <value>
    /// The client scopes.
    /// </value>
    public TableConfiguration ClientScopes
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the client secret table configuration.
    /// </summary>
    /// <value>
    /// The client secret.
    /// </value>
    public TableConfiguration ClientSecret
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the client claim table configuration.
    /// </summary>
    /// <value>
    /// The client claim.
    /// </value>
    public TableConfiguration ClientClaim
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the client IdP restriction table configuration.
    /// </summary>
    /// <value>
    /// The client IdP restriction.
    /// </value>
    public TableConfiguration ClientIdPRestriction
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the client cors origin table configuration.
    /// </summary>
    /// <value>
    /// The client cors origin.
    /// </value>
    public TableConfiguration ClientCorsOrigin
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the client property table configuration.
    /// </summary>
    /// <value>
    /// The client property.
    /// </value>
    public TableConfiguration ClientProperty
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the scope table configuration.
    /// </summary>
    /// <value>
    /// The API resource.
    /// </value>
    public TableConfiguration ApiScope
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the scope claim table configuration.
    /// </summary>
    /// <value>
    /// The API scope claim.
    /// </value>
    public TableConfiguration ApiScopeClaim
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the API resource property table configuration.
    /// </summary>
    /// <value>
    /// The client property.
    /// </value>
    public TableConfiguration ApiScopeProperty
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the identity providers table configuration.
    /// </summary>
    public TableConfiguration IdentityProvider
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the keys table configuration.
    /// </summary>
    /// <value>
    /// The keys.
    /// </value>
    public TableConfiguration Keys
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or set if EF DbContext pooling is enabled.
    /// </summary>
    public bool EnablePooling
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or set the pool size to use when DbContext pooling is enabled. If not set, the EF default is used.
    /// </summary>
    public int? PoolSize
    {
        get;
        set;
    }

    public ConfigurationStoreOptions()
    {
        DefaultSchema = null;
        IdentityResource = new TableConfiguration(TableNames.IdentityResource, SchemaNames.Identity);
        IdentityResourceClaim = new TableConfiguration(TableNames.IdentityResourceClaim, SchemaNames.Identity);
        IdentityResourceProperty = new TableConfiguration(TableNames.IdentityResourceProperty, SchemaNames.Identity);
        ApiResource = new TableConfiguration(TableNames.ApiResource, SchemaNames.Identity);
        ApiResourceSecret = new TableConfiguration(TableNames.ApiResourceSecret, SchemaNames.Identity);
        ApiResourceScope = new TableConfiguration(TableNames.ApiResourceScope, SchemaNames.Identity);
        ApiResourceClaim = new TableConfiguration(TableNames.ApiResourceClaim, SchemaNames.Identity);
        ApiResourceProperty = new TableConfiguration(TableNames.ApiResourceProperty, SchemaNames.Identity);
        Client = new TableConfiguration(TableNames.Client, SchemaNames.Identity);
        ClientGrantType = new TableConfiguration(TableNames.ClientGrantType, SchemaNames.Identity);
        ClientRedirectUri = new TableConfiguration(TableNames.ClientRedirectUri, SchemaNames.Identity);
        ClientPostLogoutRedirectUri = new TableConfiguration(TableNames.ClientPostLogoutRedirectUri, SchemaNames.Identity);
        ClientScopes = new TableConfiguration(TableNames.ClientScope, SchemaNames.Identity);
        ClientSecret = new TableConfiguration(TableNames.ClientSecret, SchemaNames.Identity);
        ClientClaim = new TableConfiguration(TableNames.ClientClaim, SchemaNames.Identity);
        ClientIdPRestriction = new TableConfiguration(TableNames.ClientIdPRestriction, SchemaNames.Identity);
        ClientCorsOrigin = new TableConfiguration(TableNames.ClientCorsOrigin, SchemaNames.Identity);
        ClientProperty = new TableConfiguration(TableNames.ClientProperty, SchemaNames.Identity);
        ApiScope = new TableConfiguration(TableNames.ApiScope, SchemaNames.Identity);
        ApiScopeClaim = new TableConfiguration(TableNames.ApiScopeClaim, SchemaNames.Identity);
        ApiScopeProperty = new TableConfiguration(TableNames.ApiScopeProperty, SchemaNames.Identity);
        IdentityProvider = new TableConfiguration(TableNames.IdentityProvider, SchemaNames.Identity);
        Keys = new TableConfiguration(TableNames.Keys, SchemaNames.Identity);
        EnablePooling = false;
    }
}