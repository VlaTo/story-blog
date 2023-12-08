namespace StoryBlog.Web.Microservices.Posts.Application.Services;

public interface IBlogPostProcessingManager
{
    Task<IBackgroundTask> QueuePostProcessingTaskAsync(Guid postKey, CancellationToken cancellationToken);
}