using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Claims;
using StoryBlog.Web.Client.Blog.Core;

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

    public static string? GetSubject(this ClaimsPrincipal? principal)
    {
        var subject = principal?.FindFirst("sub");
        return subject?.Value;
    }

    public static bool HasPermission(this ClaimsPrincipal? principal, string permission)
    {
        var claim = principal?.FindFirst("perm");
        return StringValueCollection.From(claim?.Value).Contains(permission);
    }
}