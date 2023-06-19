using System.Security.Claims;

namespace StoryBlog.Web.Common.Application.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static bool IsAuthenticated(this ClaimsPrincipal? claimsPrincipal) =>
        null != claimsPrincipal?.Identity && claimsPrincipal.Identity!.IsAuthenticated;
}