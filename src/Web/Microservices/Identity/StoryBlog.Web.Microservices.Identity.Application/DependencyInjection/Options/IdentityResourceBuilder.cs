using StoryBlog.Web.Common.Identity.Permission;
using StoryBlog.Web.Microservices.Identity.Application.Configuration;
using StoryBlog.Web.Microservices.Identity.Application.Models;
using StoryBlog.Web.Microservices.Identity.Application.Storage;

namespace StoryBlog.Web.Microservices.Identity.Application.DependencyInjection.Options;

/// <summary>
/// A builder for identity resources
/// </summary>
public class IdentityResourceBuilder
{
    private readonly IdentityResource resource;
    private bool built;

    /// <summary>
    /// Initializes a new instance of <see cref="IdentityResourceBuilder"/>.
    /// </summary>
    /*public IdentityResourceBuilder()
        : this(new IdentityResource())
    {
    }*/

    /// <summary>
    /// Initializes a new instance of <see cref="IdentityResourceBuilder"/>.
    /// </summary>
    /// <param name="resource">A preconfigured resource.</param>
    public IdentityResourceBuilder(IdentityResource resource)
    {
        this.resource = resource;
    }

    /// <summary>
    /// Creates an openid resource.
    /// </summary>
    public static IdentityResourceBuilder OpenId() =>
        IdentityResource(OidcConstants.StandardScopes.OpenId);

    /// <summary>
    /// Creates a profile resource.
    /// </summary>
    public static IdentityResourceBuilder Profile() =>
        IdentityResource(OidcConstants.StandardScopes.Profile);

    /// <summary>
    /// Builds the API resource.
    /// </summary>
    /// <returns>The built <see cref="Storage.IdentityResource"/>.</returns>
    public IdentityResource Build()
    {
        if (built)
        {
            throw new InvalidOperationException("IdentityResource already built.");
        }

        built = true;
        return resource;
    }

    /// <summary>
    /// Configures the API resource to allow all clients to access it.
    /// </summary>
    /// <returns>The <see cref="IdentityResourceBuilder"/>.</returns>
    public IdentityResourceBuilder AllowAllClients()
    {
        resource.Properties[ApplicationProfilesPropertyNames.Clients] = ApplicationProfilesPropertyValues.AllowAllApplications;
        return this;
    }

    internal IdentityResourceBuilder WithAllowedClients(string clientList)
    {
        resource.Properties[ApplicationProfilesPropertyNames.Clients] = clientList;
        return this;
    }

    internal IdentityResourceBuilder FromConfiguration()
    {
        resource.Properties[ApplicationProfilesPropertyNames.Source] = ApplicationProfilesPropertyValues.Configuration;
        return this;
    }

    internal IdentityResourceBuilder FromDefault()
    {
        resource.Properties[ApplicationProfilesPropertyNames.Source] = ApplicationProfilesPropertyValues.Default;
        return this;
    }

    internal static IdentityResourceBuilder IdentityResource(string name)
    {
        var identityResource = GetResource(name);
        return new IdentityResourceBuilder(identityResource);
    }

    private static IdentityResource GetResource(string name)
    {
        return name switch
        {
            OidcConstants.StandardScopes.OpenId => new IdentityResources.OpenId(),
            OidcConstants.StandardScopes.Profile => new IdentityResources.Profile(),
            /*
            IdentityServerConstants.StandardScopes.Address => new IdentityResources.Address(),
            IdentityServerConstants.StandardScopes.Email => new IdentityResources.Email(),
            IdentityServerConstants.StandardScopes.Phone => new IdentityResources.Phone(),
            */
            _ => throw new InvalidOperationException("Invalid identity resource type.")
        };
    }
}