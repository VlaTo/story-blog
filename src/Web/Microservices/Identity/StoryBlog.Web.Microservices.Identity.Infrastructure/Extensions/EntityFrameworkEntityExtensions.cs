using StoryBlog.Web.Microservices.Identity.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Extensions;

internal static class EntityFrameworkEntityExtensions
{
    public static Application.Storage.ApiResource ToModel(this ApiResource source)
    {
        var resource = new Application.Storage.ApiResource(source.Name, source.DisplayName)
        {
            Enabled = source.Enabled,
            Description = source.Description,
            RequireResourceIndicator = source.RequireResourceIndicator,
            ShowInDiscoveryDocument = source.ShowInDiscoveryDocument
        };

        resource
            .UseScopes(source.Scopes)
            .UseApiSecrets(source.Secrets)
            .UseAllowedAccessTokenSigningAlgorithms(source.AllowedAccessTokenSigningAlgorithms);

        resource.UseProperties(source.Properties);
        resource.UseClaims(source.UserClaims);

        return resource;
    }
    
    public static Application.Storage.IdentityResource ToModel(this IdentityResource source)
    {
        var resource = new Application.Storage.IdentityResource(source.Name, source.DisplayName)
        {
            Description = source.Description,
            Emphasize = source.Emphasize,
            Enabled = source.Enabled,
            Required = source.Required,
            ShowInDiscoveryDocument = source.ShowInDiscoveryDocument
        };

        resource.UseProperties(source.Properties);
        resource.UseClaims(source.UserClaims);

        return resource;
    }
    
    public static Application.Storage.ApiScope ToModel(this ApiScope source)
    {
        var scope = new Application.Storage.ApiScope(source.Name, source.DisplayName)
        {
            Description = source.Description,
            Emphasize = source.Emphasize,
            Enabled = source.Enabled,
            Required = source.Required,
            ShowInDiscoveryDocument = source.ShowInDiscoveryDocument
        };

        scope.UseProperties(source.Properties);
        scope.UseClaims(source.UserClaims);

        return scope;
    }

    public static Application.Storage.Client ToModel(this Client source)
    {
        var client = new Application.Storage.Client
        {
            ClientId = source.ClientId,
            ClientName = source.ClientName,
            ClientUri = source.ClientUri,
            Description = source.Description,
            Enabled = source.Enabled,
            ProtocolType = source.ProtocolType,
            RequireClientSecret = source.RequireClientSecret
        };

        client
            .UseClientSecrets(source.ClientSecrets)
            .UseAllowedGrantTypes(source.AllowedGrantTypes)
            .UseRedirectUris(source.RedirectUris)
            .UseAllowedScopes(source.AllowedScopes)
            .UseAllowedIdentityTokenSigningAlgorithms(source.AllowedIdentityTokenSigningAlgorithms)
            .UseAllowedCorsOrigins(source.AllowedCorsOrigins);
        client.UseProperties(source.Properties);
        client.UseClaims(source.Claims);

        return client;
    }

    public static Application.Storage.PersistedGrant ToModel(this PersistedGrant source)
    {
        var grant = new Application.Storage.PersistedGrant
        {
            Key = source.Key,
            Description = source.Description,
            Type = source.Type,
            ClientId = source.ClientId,
            SessionId = source.SessionId,
            SubjectId = source.SubjectId,
            Data = source.Data ?? String.Empty
        };

        return grant;
    }

    public static PersistedGrant ToEntity(this Application.Storage.PersistedGrant source)
    {
        var grant = new PersistedGrant
        {
            Key = source.Key,
            Type = source.Type,
            Description = source.Description,
            ClientId = source.ClientId,
            SessionId = source.SessionId,
            SubjectId = source.SubjectId,
            Data = source.Data,
            CreationTime = source.CreationTime,
            ConsumedTime = source.ConsumedTime,
            Expiration = source.Expiration
        };

        return grant;
    }

    /// <summary>
    /// Updates an entity from a model.
    /// </summary>
    /// <param name="grant"></param>
    /// <param name="source">The entity.</param>
    public static void UpdateEntity(this Application.Storage.PersistedGrant grant, PersistedGrant source)
    {
        grant.Key = source.Key;
        grant.Type = source.Type;
        grant.Description = source.Description;
        grant.ClientId = source.ClientId;
    }
}