using StoryBlog.Web.Common.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Comments.Domain.Specifications;

public sealed class FindChildrenCommentsForPost : SpecificationBase<Entities.Comment>
{
    public FindChildrenCommentsForPost(Guid postKey, Guid parentKey)
    {
        Criteria = entity => entity.PostKey == postKey && entity.Parent != null && entity.Parent.Key == parentKey && entity.DeletedAt == null;
        Includes.Add(x => x.Parent);
        OrderBy.Add(entity => entity.CreateAt);
    }
}