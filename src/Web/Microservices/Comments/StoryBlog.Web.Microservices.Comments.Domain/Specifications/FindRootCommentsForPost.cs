using StoryBlog.Web.Common.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Comments.Domain.Specifications;

public sealed class FindRootCommentsForPost : SpecificationBase<Entities.Comment>
{
    public FindRootCommentsForPost(Guid postKey, int pageNumber, int pageSize)
    {
        Criteria = entity => entity.PostKey == postKey && null == entity.ParentId && null == entity.DeletedAt;
        OrderBy.Add(entity => entity.CreateAt);

        if (0 < pageNumber)
        {
            Skip = (pageNumber - 1) * pageSize;
            Take = pageSize;
        }
    }
}