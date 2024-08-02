using System.Net.Http.Headers;

namespace StoryBlog.Web.Identity.Client.Extensions;

public static class HttpRequestMessageExtensions
{
    public static void SetToken(this HttpRequestMessage request, string scheme, string token)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue(scheme, token);
    }

    public static void SetBearerToken(this HttpRequestMessage request, string token)
    {
        request.SetToken(OidcConstants.AuthenticationSchemes.AuthorizationHeaderBearer, token);
    }
}