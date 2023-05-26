using StoryBlog.Web.Common.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Specifications;

internal sealed class FindKeyByIdAndUse : SpecificationBase<Domain.Entities.Key>
{
    public FindKeyByIdAndUse(string id, string use)
    {
        Criteria = x => x.Use == use && x.Id == id;
    }
}