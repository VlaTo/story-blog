using StoryBlog.Web.Common.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Specifications;

internal sealed class QueryKeysByUse : SpecificationBase<Domain.Entities.Key>
{
    public QueryKeysByUse(string use)
    {
        Criteria = x => x.Use == use;
        Options = SpecificationQueryOptions.NoTracking;
    }
}