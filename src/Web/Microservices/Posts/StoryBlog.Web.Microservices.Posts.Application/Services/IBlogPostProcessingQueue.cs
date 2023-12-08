using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Result;

namespace StoryBlog.Web.Microservices.Posts.Application.Services;

internal interface IBlogPostProcessingQueue
{
    ValueTask<Result<IBackgroundTask, TaskCancelled>> DequeueTaskAsync(CancellationToken cancellationToken);
    
    ValueTask EnqueueTaskAsync(Guid taskKey, Guid postKey, CancellationToken cancellationToken);
}