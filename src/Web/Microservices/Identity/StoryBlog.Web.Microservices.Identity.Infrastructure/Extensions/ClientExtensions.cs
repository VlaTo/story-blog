using StoryBlog.Web.Microservices.Identity.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Extensions;

internal static class ClientExtensions
{
    public static void UseProperties(this Application.Storage.Client client, IList<ClientProperty> properties)
    {
        var dictionary = new Dictionary<string, string>(properties.Count);

        foreach (var clientProperty in properties)
        {
            dictionary.Add(clientProperty.Key, clientProperty.Value);
        }

        client.Properties = dictionary;
    }

    public static void UseProperties(this Application.Storage.Client client, IDictionary<string, string> properties)
    {
        var dictionary = new Dictionary<string, string>(properties.Count);

        foreach (var (key, value) in properties)
        {
            dictionary.Add(key, value);
        }

        client.Properties = dictionary;
    }

    public static void UseClaims(this Application.Storage.Client client, IList<ClientClaim> claims)
    {
        ;
    }

    public static Application.Storage.Client UseClientSecrets(this Application.Storage.Client client, IList<ClientSecret> secrets)
    {
        var clientSecrets = new Application.Storage.Secret[secrets.Count];

        for (var index = 0; index < secrets.Count; index++)
        {
            var secret = secrets[index];
            clientSecrets[index] = new Application.Storage.Secret(secret.Value, secret.Description, secret.Expiration);
        }

        client.ClientSecrets = clientSecrets;

        return client;
    }
    
    public static Application.Storage.Client UseAllowedGrantTypes(this Application.Storage.Client client, IList<ClientGrantType> grantTypes)
    {
        client.AllowedGrantTypes = new HashSet<string>(grantTypes.Count);

        for (var index = 0; index < grantTypes.Count; index++)
        {
            client.AllowedGrantTypes.Add(grantTypes[index].GrantType);
        }

        return client;
    }
    
    public static Application.Storage.Client UseRedirectUris(this Application.Storage.Client client, IList<ClientRedirectUri> redirectUris)
    {
        client.RedirectUris = new HashSet<string>(redirectUris.Count);

        for (var index = 0; index < redirectUris.Count; index++)
        {
            var source = redirectUris[index];
            client.RedirectUris.Add(source.RedirectUri);
        }

        return client;
    }
    
    public static Application.Storage.Client UseAllowedCorsOrigins(this Application.Storage.Client client, IList<ClientCorsOrigin> corsOrigins)
    {
        client.AllowedCorsOrigins = new HashSet<string>(corsOrigins.Count);

        for (var index = 0; index < corsOrigins.Count; index++)
        {
            var source = corsOrigins[index];
            client.AllowedCorsOrigins.Add(source.Origin);
        }

        return client;
    }
    
    public static Application.Storage.Client UseAllowedScopes(this Application.Storage.Client client, IList<ClientScope> scopes)
    {
        client.AllowedScopes = new HashSet<string>(scopes.Count);

        for (var index = 0; index < scopes.Count; index++)
        {
            var scope = scopes[index];
            client.AllowedScopes.Add(scope.Scope);
        }

        return client;
    }
    
    public static Application.Storage.Client UseAllowedIdentityTokenSigningAlgorithms(this Application.Storage.Client client, string signingAlgorithms)
    {
        client.AllowedIdentityTokenSigningAlgorithms = signingAlgorithms.Split(
            new[] { ' ', ',' },
            StringSplitOptions.RemoveEmptyEntries
        );

        return client;
    }
}