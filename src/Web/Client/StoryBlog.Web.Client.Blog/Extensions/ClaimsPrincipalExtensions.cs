using System.Security.Claims;

namespace StoryBlog.Web.Client.Blog.Extensions;

internal static class ClaimsPrincipalExtensions
{
    public static bool IsAuthenticated(this ClaimsPrincipal principal)
    {
        foreach (var identity in principal.Identities)
        {
            if (identity.IsAuthenticated)
            {
                return true;
            }
        }

        return false;
    }
}