using IdentityModel;
using System.Security.Claims;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Extensions;

internal static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="principal"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static string GetSubjectId(this ClaimsPrincipal principal)
    {
        var claim = principal.FindFirst(JwtClaimTypes.Subject);

        if (null == claim)
        {
            throw new InvalidOperationException("Missing subject id for principal in authentication ticket.");
        }

        return claim.Value;
    }
}