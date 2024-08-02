using System.Net.Http.Headers;
using StoryBlog.Web.Identity.Client.Requests;
using StoryBlog.Web.Identity.Client.Responses;

namespace StoryBlog.Web.Identity.Client.Extensions;

public static class HttpClientExtensions
{
    public static void SetBearerToken(this HttpClient client, string token)
    {
        client.SetToken(OidcConstants.AuthenticationSchemes.AuthorizationHeaderBearer, token);
    }

    public static void SetToken(this HttpClient client, string scheme, string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);
    }

    public static async Task<DiscoveryDocumentResponse> GetDiscoveryDocumentAsync(
        this HttpClient httpClient,
        string address,
        CancellationToken cancellationToken)
    {
        return await httpClient
            .GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest(address), cancellationToken)
            .ConfigureAwait(false);
    }
}