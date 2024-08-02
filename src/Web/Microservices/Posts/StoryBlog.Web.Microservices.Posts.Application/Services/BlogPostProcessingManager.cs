using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Result;
using System.Threading.Channels;
using StoryBlog.Web.Microservices.Posts.Application.Extensions;

namespace StoryBlog.Web.Microservices.Posts.Application.Services;

internal sealed class BlogPostProcessingManager : IBlogPostProcessingManager, IBlogPostProcessingQueue
{
    private const int DefaultCapacity = 16;

    private readonly ILogger<BlogPostProcessingManager> logger;
    private readonly Channel<BackgroundTask> channel;

    public bool HasPendingTasks => channel.Reader is { CanCount: true, Count: > 0 };

    public BlogPostProcessingManager(ILogger<BlogPostProcessingManager> logger)
    {
        this.logger = logger;
        
        channel = Channel.CreateBounded<BackgroundTask>(new BoundedChannelOptions(DefaultCapacity)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleWriter = false,
            SingleReader = true
        });
    }

    public async Task<IBackgroundTask> QueuePostProcessingTaskAsync(Guid postKey, CancellationToken cancellationToken)
    {
        var backgroundTask = new BackgroundTask(Guid.NewGuid(), postKey);

        logger.QueuePostProcessingTask(backgroundTask);

        await channel.Writer.WriteAsync(backgroundTask, cancellationToken);
        
        return backgroundTask;
    }

    public async ValueTask<Result<IBackgroundTask, TaskCancelled>> DequeueTaskAsync(CancellationToken cancellationToken)
    {
        try
        {
            var backgroundTask = await channel.Reader.ReadAsync(cancellationToken);

            return new Result<IBackgroundTask, TaskCancelled>(backgroundTask);
        }
        catch (TaskCanceledException)
        {
            return new Result<IBackgroundTask, TaskCancelled>(TaskCancelled.Instance);
        }
        catch (Exception exception)
        {
            return new Result<IBackgroundTask, TaskCancelled>(exception);
        }
    }

    public async ValueTask EnqueueTaskAsync(Guid taskKey, Guid postKey, CancellationToken cancellationToken)
    {
        var backgroundTask = new BackgroundTask(taskKey, postKey);
        await channel.Writer.WriteAsync(backgroundTask, cancellationToken);
    }

    #region class: BackgroundTask

    private sealed class BackgroundTask : IBackgroundTask
    {
        public Guid TaskKey
        {
            get;
        }

        public Guid PostKey
        {
            get;
        }

        public BackgroundTask(Guid taskKey, Guid postKey)
        {
            TaskKey = taskKey;
            PostKey = postKey;
        }
    }

    #endregion
}