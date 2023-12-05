using StoryBlog.Web.Common.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Posts.Domain.Specifications;

public sealed class AllPendingPostProcessingTasksSpecification : SpecificationBase<Entities.PostProcessTask>
{
    public AllPendingPostProcessingTasksSpecification()
    {
        Criteria = entity => Entities.PostProcessStatus.Pending == entity.Status;
        Options = SpecificationQueryOptions.NoTracking;
    }
}