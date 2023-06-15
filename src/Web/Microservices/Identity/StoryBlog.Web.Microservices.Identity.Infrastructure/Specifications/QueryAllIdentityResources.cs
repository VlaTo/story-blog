using StoryBlog.Web.Common.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Specifications;

internal sealed class QueryAllIdentityResources : SpecificationBase<Domain.Entities.IdentityResource>
{
    public QueryAllIdentityResources()
    {
        Includes.Add(x => x.UserClaims);
        Includes.Add(x => x.Properties);
        Options = SpecificationQueryOptions.NoTracking;
    }
}