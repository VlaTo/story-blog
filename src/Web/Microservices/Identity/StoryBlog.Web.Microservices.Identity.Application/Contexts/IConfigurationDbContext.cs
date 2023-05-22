using Microsoft.EntityFrameworkCore;

namespace StoryBlog.Web.Microservices.Identity.Application.Contexts;

/// <summary>
/// Abstraction for the configuration context.
/// </summary>
/// <seealso cref="System.IDisposable" />
public interface IConfigurationDbContext
{
    /// <summary>
    /// Gets or sets the clients.
    /// </summary>
    /// <value>
    /// The clients.
    /// </value>
    /*DbSet<Domain.Entities.Client> Clients
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the clients' CORS origins.
    /// </summary>
    /// <value>
    /// The clients CORS origins.
    /// </value>
    DbSet<Domain.Entities.ClientCorsOrigin> ClientCorsOrigins
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the identity resources.
    /// </summary>
    /// <value>
    /// The identity resources.
    /// </value>
    DbSet<Domain.Entities.IdentityResource> IdentityResources
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the API resources.
    /// </summary>
    /// <value>
    /// The API resources.
    /// </value>
    DbSet<Domain.Entities.ApiResource> ApiResources
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the scopes.
    /// </summary>
    /// <value>
    /// The identity resources.
    /// </value>
    DbSet<Domain.Entities.ApiScope> ApiScopes
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the identity providers.
    /// </summary>
    /// <value>
    /// The identity providers.
    /// </value>
    DbSet<Domain.Entities.IdentityProvider> IdentityProviders
    {
        get;
        set;
    }*/
}