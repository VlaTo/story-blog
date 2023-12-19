using StoryBlog.Web.Common.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Posts.Domain.Specifications;

public sealed class PagedPostsSpecification : SpecificationBase<Entities.Post>
{
    public PagedPostsSpecification(bool authenticated, int pageNumber, int pageSize)
    {
        Criteria = entity => null == entity.DeletedAt && (entity.IsPublic || authenticated);
        Includes.Add(x => x.Slug);
        Includes.Add(x => x.Content);
        OrderBy.Add(entity => entity.CreateAt);
        OrderBy.Add(entity => entity.Title);
        Skip = (pageNumber - 1) * pageSize;
        Take = pageSize;
    }
}