using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using StoryBlog.Web.Common.Identity.Permission;

namespace StoryBlog.Web.Client.Blog.Core.Authorization.Requirements;

public sealed class PermissionsClaimAuthorizationRequirement : AuthorizationHandler<PermissionsClaimAuthorizationRequirement>, IAuthorizationRequirement
{
    public IEnumerable<string> AllowedPermissions
    {
        get;
    }

    public PermissionsClaimAuthorizationRequirement(IEnumerable<string> allowedPermissions)
    {
        AllowedPermissions = allowedPermissions;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionsClaimAuthorizationRequirement requirement)
    {
        if (null != context.User && requirement.AllowedPermissions.Any())
        {
            //var userClaims = context.User.Claims.ToArray();
            //var claim = userClaims.FirstOrDefault(
            var claim = context.User.Claims.FirstOrDefault(
                x => String.Equals(x.Type, ClaimIdentityTypes.Permission, StringComparison.Ordinal)
            );

            if (null != claim && String.Equals(claim.ValueType, ClaimValueTypes.String))
            {
                var values = ClaimStringValueCollection.Create(claim.Value);
                    
                if (values.Contains(requirement.AllowedPermissions))
                {
                    context.Succeed(requirement);
                }
            }
        }

        return Task.CompletedTask;
    }
}