using StoryBlog.Web.Common.Domain.Entities;
using StoryBlog.Web.Common.Domain.Specifications;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Domain.Specifications;

public sealed class AllPostsSpecification : SpecificationBase<Post>
{
    public AllPostsSpecification(bool isAuthenticated, string? userId)
    {
        Criteria = entity => VisibilityStatus.Public == entity.VisibilityStatus
                             || (VisibilityStatus.ForAuthenticated == entity.VisibilityStatus && isAuthenticated)
                             || (VisibilityStatus.Private == entity.VisibilityStatus && entity.AuthorId == userId);
    }
}