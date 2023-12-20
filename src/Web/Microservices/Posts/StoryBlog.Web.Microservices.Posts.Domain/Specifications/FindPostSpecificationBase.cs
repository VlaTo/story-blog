using StoryBlog.Web.Common.Domain.Specifications;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Domain.Specifications;

public class FindPostSpecificationBase : SpecificationBase<Post>
{
    public FindPostSpecificationBase(bool includeAll)
    {
        if (includeAll)
        {
            Includes.Add(entity => entity.Slug);
            Includes.Add(entity => entity.CommentsCounter);
            Includes.Add(entity => entity.Content);
        }
    }
}