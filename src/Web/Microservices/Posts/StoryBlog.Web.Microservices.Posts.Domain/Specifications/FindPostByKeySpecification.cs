using StoryBlog.Web.Common.Domain.Specifications;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Domain.Specifications;

public sealed class FindPostByKeySpecification : SpecificationBase<Post>
{
    public FindPostByKeySpecification(Guid key, bool includeAll)
    {
        Criteria = entity => entity.Key == key;

        if (includeAll)
        {
            Includes.Add(entity => entity.Slug);
            Includes.Add(entity => entity.CommentsCounter);
            Includes.Add(entity => entity.Content);
        }
    }
}