using StoryBlog.Web.Common.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Posts.Domain.Specifications;

public sealed class AllPostsSpecification :SpecificationBase<Entities.Post>
{
    public AllPostsSpecification(bool authenticated)
    {
        Criteria = entity => null == entity.DeletedAt && (entity.IsPublic || authenticated);
    }
}