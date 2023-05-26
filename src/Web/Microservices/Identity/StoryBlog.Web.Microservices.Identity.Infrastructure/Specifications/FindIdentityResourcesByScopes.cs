using StoryBlog.Web.Common.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Specifications;

internal sealed class FindIdentityResourcesByScopes : SpecificationBase<Domain.Entities.IdentityResource>
{
    public FindIdentityResourcesByScopes(string[] scopes)
    {
        Criteria = resource => scopes.Contains(resource.Name);
        Includes.Add(resource => resource.UserClaims);
        Includes.Add(resource => resource.Properties);
        Options = SpecificationQueryOptions.NoTracking;
    }
}