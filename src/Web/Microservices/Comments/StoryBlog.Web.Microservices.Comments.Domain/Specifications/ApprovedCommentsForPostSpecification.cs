using StoryBlog.Web.Common.Domain.Entities;
using StoryBlog.Web.Common.Domain.Specifications;
using StoryBlog.Web.Microservices.Comments.Domain.Entities;

namespace StoryBlog.Web.Microservices.Comments.Domain.Specifications;

public sealed class ApprovedCommentsForPostSpecification : SpecificationBase<Comment>
{
    public ApprovedCommentsForPostSpecification(Guid postKey)
    {
        Criteria = entity => entity.PostKey == postKey && PublicationStatus.Approved == entity.PublicationStatus;
    }
}