using StoryBlog.Web.Microservices.Identity.Application.Validation.Contexts;
using System.Security.Claims;

namespace StoryBlog.Web.Microservices.Identity.Application.Extensions;

public static class ProfileDataRequestContextExtensions
{
    /// <summary>
    /// Filters the claims based on requested claim types.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="claims">The claims.</param>
    /// <returns></returns>
    public static List<Claim> FilterClaims(this ProfileDataRequestContext context, IEnumerable<Claim>? claims)
    {
        if (null == claims)
        {
            throw new ArgumentNullException(nameof(claims));
        }

        return claims
            .Where(claim => context.RequestedClaimTypes.Contains(claim.Type))
            .ToList();
    }

    /// <summary>
    /// Filters the claims based on the requested claim types and then adds them to the IssuedClaims collection.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="claims">The claims.</param>
    public static void AddRequestedClaims(this ProfileDataRequestContext context, IEnumerable<Claim> claims)
    {
        if (context.RequestedClaimTypes.Any())
        {
            context.IssuedClaims.AddRange(context.FilterClaims(claims));
        }
    }
}