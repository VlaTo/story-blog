using StoryBlog.Web.Common.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Comments.Domain.Specifications;

public sealed class ChildCommentsForPostCommentSpecification : SpecificationBase<Entities.Comment>
{
    public ChildCommentsForPostCommentSpecification(Guid postKey, Guid parentKey)
    {
        Criteria = entity => entity.PostKey == postKey && entity.Parent != null && entity.Parent.Key == parentKey;
        Includes.Add(x => x.Parent);
        OrderBy.Add(entity => entity.CreateAt);
    }
}