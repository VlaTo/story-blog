using Microsoft.AspNetCore.Authorization;
using StoryBlog.Web.Common.Identity.Permission;
using System.Security.Claims;

namespace StoryBlog.Web.Common.Authorization.Requirements;

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
        if (null != context.User)
        {
            bool found = false;

            if (requirement.AllowedPermissions.Any())
            {
                var claim = context.User.Claims.FirstOrDefault(x => string.Equals(x.Type, ClaimIdentityTypes.Permission, StringComparison.Ordinal));

                if (null != claim && String.Equals(claim.ValueType, ClaimValueTypes.String))
                {
                    var values = ClaimStringValueCollection.Create(claim.Value);
                    found = values.Contains(requirement.AllowedPermissions);
                }
            }

            if (found)
            {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}