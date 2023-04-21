using StoryBlog.Web.Common.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Comments.Domain.Specifications;

public sealed class FindRootCommentsForPostSpecification : SpecificationBase<Entities.Comment>
{
    public FindRootCommentsForPostSpecification(Guid postKey, int pageNumber = -1, int pageSize = 0, bool includeAll = false)
    {
        Criteria = entity => entity.PostKey == postKey && null == entity.ParentId && null == entity.DeletedAt;

        if (includeAll)
        {
            Includes.Add(x => x.Comments);
            //Includes.Add(x => x.Parent);
        }

        OrderBy.Add(entity => entity.CreateAt);

        if (0 < pageNumber)
        {
            Skip = (pageNumber - 1) * pageSize;
            Take = pageSize;
        }
    }
}