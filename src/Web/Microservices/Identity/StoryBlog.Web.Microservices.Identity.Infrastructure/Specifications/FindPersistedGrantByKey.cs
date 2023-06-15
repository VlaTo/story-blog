using StoryBlog.Web.Common.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Specifications;

internal sealed class FindPersistedGrantByKey : SpecificationBase<Domain.Entities.PersistedGrant>
{
    public FindPersistedGrantByKey(string grantKey)
    {
        Criteria = x => x.Key == grantKey;
        Options = SpecificationQueryOptions.NoTracking;
    }
}