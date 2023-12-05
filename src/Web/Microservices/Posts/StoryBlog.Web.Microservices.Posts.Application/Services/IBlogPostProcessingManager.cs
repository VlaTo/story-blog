using StoryBlog.Web.Common.Result;

namespace StoryBlog.Web.Microservices.Posts.Application.Services;

public interface IBlogPostProcessingManager
{
    ValueTask<IBackgroundTask> AddPostTaskAsync(Guid postKey, CancellationToken cancellationToken);

    ValueTask QueuePendingPostTasksAsync(CancellationToken cancellationToken);
    
    ValueTask<Result<IBackgroundTask>> ReadPostTaskAsync(CancellationToken cancellationToken);

    ValueTask<IBackgroundTaskToken> StartProcessingAsync(IBackgroundTask backgroundTask, CancellationToken cancellationToken);
}