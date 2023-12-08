using StoryBlog.Web.Common.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Comments.Domain.Specifications;

public sealed class FindCommentsForPost : SpecificationBase<Entities.Comment>
{
    public FindCommentsForPost(Guid postKey)
    {
        Criteria = entity => entity.PostKey == postKey && null == entity.ParentId && null == entity.DeletedAt;
    }
}