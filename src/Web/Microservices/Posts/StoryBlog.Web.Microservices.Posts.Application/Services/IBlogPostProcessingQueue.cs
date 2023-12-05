using StoryBlog.Web.Common.Result;

namespace StoryBlog.Web.Microservices.Posts.Application.Services;

public interface IBlogPostProcessingQueue
{
    ValueTask EnqueueTaskAsync(IBackgroundTask backgroundTask, CancellationToken cancellationToken);

    ValueTask<Result<IBackgroundTask>> DequeueTaskAsync(CancellationToken cancellationToken);
}