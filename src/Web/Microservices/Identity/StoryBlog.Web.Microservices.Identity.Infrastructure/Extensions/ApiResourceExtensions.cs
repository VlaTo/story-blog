using StoryBlog.Web.Microservices.Identity.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Extensions;

internal static class ApiResourceExtensions
{
    public static Application.Storage.ApiResource UseScopes(this Application.Storage.ApiResource apiResource, IList<ApiResourceScope> scopes)
    {
        apiResource.Scopes = new HashSet<string>(scopes.Count);

        for (var index = 0; index < scopes.Count; index++)
        {
            apiResource.Scopes.Add(scopes[index].Scope);
        }

        return apiResource;
    }
    
    public static Application.Storage.ApiResource UseApiSecrets(this Application.Storage.ApiResource apiResource, IList<ApiResourceSecret> secrets)
    {
        var apiSecrets = new List<Application.Storage.Secret>(secrets.Count);

        for (var index = 0; index < secrets.Count; index++)
        {
            var secret = secrets[index];
            apiSecrets[index] = new Application.Storage.Secret(secret.Value, secret.Description, secret.Expiration);
        }

        apiResource.ApiSecrets = apiSecrets;

        return apiResource;
    }

    public static Application.Storage.ApiResource UseAllowedAccessTokenSigningAlgorithms(this Application.Storage.ApiResource resource, string algorithm)
    {
        var algorithms = algorithm.Split(' ');

        resource.AllowedAccessTokenSigningAlgorithms = new HashSet<string>(algorithms);

        return resource;
    }
}