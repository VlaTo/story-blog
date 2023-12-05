using StoryBlog.Web.Common.Domain.Specifications;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Domain.Specifications;

public class FindTaskByKeySpecification : SpecificationBase<PostProcessTask>
{
    public FindTaskByKeySpecification(Guid taskKey)
    {
        Criteria = entity => entity.Key == taskKey;
    }
}