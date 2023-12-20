using StoryBlog.Web.Common.Domain.Entities;
using StoryBlog.Web.Common.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Posts.Domain.Specifications;

public sealed class QueryPostsPageSpecification : SpecificationBase<Entities.Post>
{
    public QueryPostsPageSpecification(bool isAuthenticated, string? userId, int pageNumber, int pageSize)
    {
        Criteria = entity => VisibilityStatus.Public == entity.VisibilityStatus
                             || (VisibilityStatus.ForAuthenticated == entity.VisibilityStatus && isAuthenticated)
                             || (VisibilityStatus.Private == entity.VisibilityStatus && entity.AuthorId == userId);

        Includes.Add(x => x.Slug);
        Includes.Add(x => x.Content);
        
        OrderBy.Add(entity => entity.CreateAt);
        OrderBy.Add(entity => entity.Title);
        
        Skip = (pageNumber - 1) * pageSize;
        
        Take = pageSize;
    }
}