using Microsoft.AspNetCore.Authorization;
using StoryBlog.Web.Client.Blog.Core.Authorization.Requirements;

namespace StoryBlog.Web.Client.Blog.Extensions;

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