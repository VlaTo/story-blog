using StoryBlog.Web.Common.Domain.Specifications;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Domain.Specifications;

public sealed class FindPostBySlugSpecification : SpecificationBase<Post>
{
    public FindPostBySlugSpecification(string slug)
    {
        Criteria = entity => entity.Slug.Text == slug;
        Includes.Add(entity => entity.Slug);
    }
}