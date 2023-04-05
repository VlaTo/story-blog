namespace StoryBlog.Web.Microservices.Posts.Domain.Specifications;

public class AllAvailablePostsSpecification : SpecificationBase<Entities.Post>
{
    public AllAvailablePostsSpecification(int pageNumber, int pageSize)
    {
        Criteria = entity => null == entity.DeletedAt;
        Includes.Add(x => x.Slug);
        OrderBy.Add(entity => entity.CreateAt);
        OrderBy.Add(entity => entity.Title);
        Skip = (pageNumber - 1) * pageSize;
        Take = pageSize;
    }
}