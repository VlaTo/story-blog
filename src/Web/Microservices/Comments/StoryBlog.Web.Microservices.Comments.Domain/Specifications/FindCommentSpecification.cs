using StoryBlog.Web.Common.Domain.Specifications;
using StoryBlog.Web.Microservices.Comments.Domain.Entities;

namespace StoryBlog.Web.Microservices.Comments.Domain.Specifications;

public sealed class FindCommentSpecification : SpecificationBase<Comment>
{
    public FindCommentSpecification(Guid key)
    {
        Criteria = entity => entity.Key == key;
    }
}