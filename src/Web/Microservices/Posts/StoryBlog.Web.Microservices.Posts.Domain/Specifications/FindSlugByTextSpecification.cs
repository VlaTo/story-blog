using StoryBlog.Web.Common.Domain.Specifications;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Domain.Specifications;

public sealed class FindSlugByTextSpecification : SpecificationBase<Post>
{
    public FindSlugByTextSpecification(string text)
    {
        Criteria = entity => entity.Slug.Text == text;
        Includes.Add(x => x.Slug);
    }
}