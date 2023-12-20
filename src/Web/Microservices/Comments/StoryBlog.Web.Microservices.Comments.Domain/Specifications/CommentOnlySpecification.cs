using StoryBlog.Web.Common.Domain.Specifications;
using StoryBlog.Web.Microservices.Comments.Domain.Entities;

namespace StoryBlog.Web.Microservices.Comments.Domain.Specifications;

public sealed class CommentOnlySpecification : SpecificationBase<Comment>
{
    public CommentOnlySpecification(Guid commentKey)
    {
        Criteria = entity => entity.Key == commentKey;
    }
}