using StoryBlog.Web.Common.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Specifications;

internal sealed class QueryAllApiResources : SpecificationBase<Domain.Entities.ApiResource>
{
    public QueryAllApiResources()
    {
        Includes.Add(x => x.Secrets);
        Includes.Add(x => x.Scopes);
        Includes.Add(x => x.UserClaims);
        Includes.Add(x => x.Properties);
        Options = SpecificationQueryOptions.NoTracking;
    }
}