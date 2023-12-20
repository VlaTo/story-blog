using StoryBlog.Web.Common.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Comments.Domain.Specifications;

public class RootCommentsForPostSpecification : SpecificationBase<Entities.Comment>
{
    public RootCommentsForPostSpecification(Guid postKey)
    {
        Criteria = entity => entity.PostKey == postKey && null == entity.ParentId;
    }
}