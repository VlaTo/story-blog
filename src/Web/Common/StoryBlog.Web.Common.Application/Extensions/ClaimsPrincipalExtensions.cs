using StoryBlog.Web.Common.Identity.Permission;
using System.Security.Claims;

namespace StoryBlog.Web.Common.Application.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static bool IsAuthenticated(this ClaimsPrincipal? claimsPrincipal) =>
        null != claimsPrincipal?.Identity && claimsPrincipal.Identity!.IsAuthenticated;

    public static bool HasPermission(this ClaimsPrincipal? claimsPrincipal, string permission)
    {
        return null != claimsPrincipal && CheckPermissions(claimsPrincipal, true, new[] { permission });
    }

    public static bool HasAnyPermissions(this ClaimsPrincipal? claimsPrincipal, params string[] permissions)
    {
        return null != claimsPrincipal && CheckPermissions(claimsPrincipal, true, permissions);
    }

    public static bool HasAllPermissions(this ClaimsPrincipal? claimsPrincipal, params string[] permissions)
    {
        return null != claimsPrincipal && CheckPermissions(claimsPrincipal, false, permissions);
    }

    public static string? GetSubject(this ClaimsPrincipal? claimsPrincipal)
    {
        var claim = claimsPrincipal?.Claims.FirstOrDefault(x => String.Equals(x.Type, ClaimTypes.NameIdentifier));
        return claim?.Value;
    }

    public static string? GetName(this ClaimsPrincipal? claimsPrincipal)
    {
        if (null != claimsPrincipal)
        {
            foreach (var identity in claimsPrincipal.Identities)
            {
                if (String.IsNullOrEmpty(identity.Name))
                {
                    continue;
                }

                return identity.Name;
            }
        }
        
        return null;
    }

    private static bool CheckPermissions(ClaimsPrincipal claimsPrincipal, bool anyOrAll, string[] permissions)
    {
        bool Test(Claim claim) => permissions.Any(permission => String.Equals(permission, claim.Value));
        var claimPermissions = claimsPrincipal.Claims.Where(x => String.Equals(x.Type, ClaimIdentityTypes.Permission));
        return anyOrAll ? claimPermissions.Any(Test) : claimPermissions.All(Test);
    }
}