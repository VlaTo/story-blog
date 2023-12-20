namespace StoryBlog.Web.Microservices.Comments.Domain.Specifications;

public sealed class RootCommentsForPostByPagesSpecification : RootCommentsForPostSpecification
{
    public RootCommentsForPostByPagesSpecification(Guid postKey, int pageNumber, int pageSize)
        : base(postKey)
    {
        OrderBy.Add(entity => entity.CreateAt);

        if (0 < pageNumber)
        {
            Skip = (pageNumber - 1) * pageSize;
            Take = pageSize;
        }
    }
}