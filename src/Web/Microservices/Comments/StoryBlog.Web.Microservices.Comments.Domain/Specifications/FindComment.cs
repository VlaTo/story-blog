using StoryBlog.Web.Common.Domain.Specifications;
using StoryBlog.Web.Microservices.Comments.Domain.Entities;

namespace StoryBlog.Web.Microservices.Comments.Domain.Specifications;

public sealed class FindComment : SpecificationBase<Comment>
{
    public FindComment(Guid commentKey)
    {
        Criteria = entity => entity.Key == commentKey;
    }
}