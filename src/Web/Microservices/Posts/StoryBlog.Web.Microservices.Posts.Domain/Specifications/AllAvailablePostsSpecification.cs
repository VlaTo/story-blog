using StoryBlog.Web.Common.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Posts.Domain.Specifications;

public sealed class AllAvailablePostsSpecification : SpecificationBase<Entities.Post>
{
    public AllAvailablePostsSpecification(bool authenticated, int pageNumber, int pageSize)
    {
        Criteria = entity => null == entity.DeletedAt && (entity.IsPublic || authenticated);
        Includes.Add(x => x.Slug);
        OrderBy.Add(entity => entity.CreateAt);
        OrderBy.Add(entity => entity.Title);
        Skip = (pageNumber - 1) * pageSize;
        Take = pageSize;
    }
}