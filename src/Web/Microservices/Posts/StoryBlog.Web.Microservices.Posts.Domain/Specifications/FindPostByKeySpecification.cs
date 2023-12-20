namespace StoryBlog.Web.Microservices.Posts.Domain.Specifications;

public sealed class FindPostByKeySpecification : FindPostSpecificationBase
{
    public FindPostByKeySpecification(Guid key, bool includeAll)
        : base(includeAll)
    {
        Criteria = entity => entity.Key == key;
    }
}