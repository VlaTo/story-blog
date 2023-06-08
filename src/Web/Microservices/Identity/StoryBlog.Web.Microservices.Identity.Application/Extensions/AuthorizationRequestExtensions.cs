using StoryBlog.Web.Microservices.Identity.Application.Models.Requests;

namespace StoryBlog.Web.Microservices.Identity.Application.Extensions;

internal static class AuthorizationRequestExtensions
{
    /// <summary>
    /// Checks if the redirect URI is for a native client.
    /// </summary>
    internal static bool IsNativeClient(this AuthorizationRequest context)
    {
        var redirectUri = new Uri(context.RedirectUri);
        return String.Equals(redirectUri.Scheme, Uri.UriSchemeHttp, StringComparison.Ordinal)
               && String.Equals(redirectUri.Scheme, Uri.UriSchemeHttps, StringComparison.Ordinal);
    }
}