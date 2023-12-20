using StoryBlog.Web.Common.Domain.Specifications;
using StoryBlog.Web.Microservices.Comments.Domain.Entities;

namespace StoryBlog.Web.Microservices.Comments.Domain.Specifications;

public sealed class CommentWithParentOrChildrenSpecification : SpecificationBase<Comment>
{
    public CommentWithParentOrChildrenSpecification(Guid postKey, Guid parentKey, bool includeChildren = false, bool includeParent = false)
    {
        Criteria = entity => entity.PostKey == postKey && entity.Key == parentKey;

        if (includeParent)
        {
            Includes.Add(entity => entity.Parent);
        }

        if (includeChildren)
        {
            Includes.Add(entity => entity.Comments);
        }
    }
}