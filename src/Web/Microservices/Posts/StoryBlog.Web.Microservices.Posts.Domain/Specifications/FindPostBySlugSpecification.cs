namespace StoryBlog.Web.Microservices.Posts.Domain.Specifications;

public sealed class FindPostBySlugSpecification : FindPostSpecificationBase
{
    public FindPostBySlugSpecification(string slug, bool includeAll)
        : base(includeAll)
    {
        Criteria = entity => entity.Slug.Text == slug;
    }
}