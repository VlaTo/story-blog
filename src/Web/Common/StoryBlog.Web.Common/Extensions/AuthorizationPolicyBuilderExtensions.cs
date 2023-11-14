using Microsoft.AspNetCore.Authorization;
using StoryBlog.Web.Common.Authorization.Requirements;

namespace StoryBlog.Web.Common.Extensions;

public static class AuthorizationPolicyBuilderExtensions
{
    public static AuthorizationPolicyBuilder RequirePermissionClaim(this AuthorizationPolicyBuilder policyBuilder, params string[] permissions)
    {
        if (permissions.Any())
        {
            policyBuilder.AddRequirements(new PermissionsClaimAuthorizationRequirement(permissions));
        }

        return policyBuilder;
    }
}