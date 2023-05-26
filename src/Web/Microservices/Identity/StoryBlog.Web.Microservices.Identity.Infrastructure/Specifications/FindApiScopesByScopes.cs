using StoryBlog.Web.Common.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Specifications;

internal sealed class FindApiScopesByScopes : SpecificationBase<Domain.Entities.ApiScope>
{
    public FindApiScopesByScopes(string[] scopes)
    {
        Criteria = resource => scopes.Contains(resource.Name);
        Includes.Add(x => x.UserClaims);
        Includes.Add(x => x.Properties);
        Options = SpecificationQueryOptions.NoTracking;
    }
}