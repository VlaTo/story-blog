using StoryBlog.Web.Common.Domain.Entities;
using StoryBlog.Web.Common.Domain.Specifications;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Domain.Specifications;

public sealed class TailPostsSpecification : SpecificationBase<Post>
{
    public TailPostsSpecification(bool isAuthenticated, string? userId, DateTimeOffset dateTime, int postsCount)
    {
        Criteria = entity => (VisibilityStatus.Public == entity.VisibilityStatus
                             || (VisibilityStatus.ForAuthenticated == entity.VisibilityStatus && isAuthenticated)
                             || (VisibilityStatus.Private == entity.VisibilityStatus && entity.AuthorId == userId))
                             && entity.CreateAt > dateTime;

        Includes.Add(x => x.Slug);
        Includes.Add(x => x.Content);
        
        OrderBy.Add(entity => entity.CreateAt);
        OrderBy.Add(entity => entity.Title);
        
        Take = postsCount;
    }
}