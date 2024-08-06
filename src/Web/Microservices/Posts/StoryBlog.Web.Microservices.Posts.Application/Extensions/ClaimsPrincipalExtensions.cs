using System.Security.Claims;
using StoryBlog.Web.Common.Identity.Permission;

namespace StoryBlog.Web.Microservices.Posts.Application.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static bool TryGetClientId(this ClaimsPrincipal? principal, out string? clientId)
    {
        if (null != principal)
        {
            var clientIdClaim = principal.Claims.FirstOrDefault(x => String.Equals(x.Type, JwtClaimTypes.ClientId));

            if (null != clientIdClaim)
            {
                clientId = clientIdClaim.Value;

                return true;
            }
        }

        clientId = null;

        return false;
    }

    public static bool IsClient(this ClaimsPrincipal? principal)
    {
        if (null == principal)
        {
            return false;
        }

        return principal.TryGetClientId(out _);
    }

    public static bool ClientHasPermission(this ClaimsPrincipal? principal, string permission)
    {
        if (null == principal)
        {
            return false;
        }

        const string claimType = "client_" + ClaimIdentityTypes.Permission;

        var exists = principal.Claims.Any(
            x => String.Equals(x.Type, claimType) && String.Equals(x.Value, permission)
        );

        return exists;
    }
}