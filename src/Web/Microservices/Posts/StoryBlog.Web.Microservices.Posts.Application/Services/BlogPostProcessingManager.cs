using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Posts.Application.Models;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;
using StoryBlog.Web.Microservices.Posts.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Posts.Application.Services;

internal sealed class BlogPostProcessingManager : IBlogPostProcessingManager
{
    private readonly IServiceProvider serviceProvider;
    private readonly IBlogPostProcessingQueueProvider queueProvider;
    private readonly ILogger<BlogPostProcessingManager> logger;

    public BlogPostProcessingManager(
        IServiceProvider serviceProvider,
        IBlogPostProcessingQueueProvider queueProvider,
        ILogger<BlogPostProcessingManager> logger)
    {
        this.serviceProvider = serviceProvider;
        this.queueProvider = queueProvider;
        this.logger = logger;
    }

    public async ValueTask<IBackgroundTask> AddPostTaskAsync(Guid postKey, CancellationToken cancellationToken)
    {
        var taskKey = Guid.NewGuid();

        await using (var scope = serviceProvider.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<IAsyncUnitOfWork>();

            await using (var repository = context.GetRepository<PostProcessTask>())
            {
                var processTask = new PostProcessTask
                {
                    Key = taskKey,
                    PostKey = postKey,
                    Status = PostProcessStatus.Pending
                };

                await repository.AddAsync(processTask, cancellationToken);
                await repository.SaveChangesAsync(cancellationToken);
            }
        }

        var queue = queueProvider.GetQueue();
        var backgroundTask = new BackgroundTask(taskKey, postKey);

        await queue.EnqueueTaskAsync(backgroundTask, cancellationToken);

        return backgroundTask;
    }

    public async ValueTask QueuePendingPostTasksAsync(CancellationToken cancellationToken)
    {
        var queue = queueProvider.GetQueue();

        await using (var scope = serviceProvider.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<IAsyncUnitOfWork>();

            await using (var repository = context.GetRepository<PostProcessTask>())
            {
                var entities = await repository.QueryAsync(
                    new AllPendingPostProcessingTasksSpecification(),
                    cancellationToken
                );

                foreach (var processTask in entities)
                {
                    var backgroundTask = new BackgroundTask(processTask.Key, processTask.PostKey);
                    await queue.EnqueueTaskAsync(backgroundTask, cancellationToken);
                }
            }
        }
    }

    public async ValueTask<Result<IBackgroundTask>> ReadPostTaskAsync(CancellationToken cancellationToken)
    {
        var queue = queueProvider.GetQueue();
        var result = await queue.DequeueTaskAsync(cancellationToken);

        return result;
    }

    public async ValueTask<IBackgroundTaskToken> StartProcessingAsync(IBackgroundTask backgroundTask, CancellationToken cancellationToken)
    {
        await using (var scope = serviceProvider.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<IAsyncUnitOfWork>();

            await using (var repository = context.GetRepository<PostProcessTask>())
            {
                var specification = new FindTaskByKeySpecification(backgroundTask.TaskKey);
                var entity = await repository.FindAsync(specification, cancellationToken);

                if (null != entity)
                {
                    entity.Status = PostProcessStatus.Processing;
                    await repository.SaveChangesAsync(cancellationToken);

                    return new BackgroundTaskToken(backgroundTask.TaskKey, entity.Key, SaveProcessingTaskAsync);
                }
            }
        }

        return BackgroundTaskToken.Empty;
    }

    private async ValueTask SaveProcessingTaskAsync(Guid taskKey, PostProcessStatus status, CancellationToken cancellationToken)
    {
        await using (var scope = serviceProvider.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<IAsyncUnitOfWork>();

            await using (var repository = context.GetRepository<PostProcessTask>())
            {
                var specification = new FindTaskByKeySpecification(taskKey);
                var entity = await repository.FindAsync(specification, cancellationToken);

                if (null != entity)
                {
                    entity.Status = status;
                    await repository.SaveChangesAsync(cancellationToken);
                }
            }
        }
    }

    #region BackgroundTaskToken class

    private sealed class BackgroundTaskToken : IBackgroundTaskToken
    {
        public static readonly BackgroundTaskToken Empty;

        private Func<Guid, PostProcessStatus, CancellationToken, ValueTask>? callback;
        private bool disposed;

        public Guid TaskKey
        {
            get;
        }

        public Guid PostKey
        {
            get;
        }

        public BackgroundTaskToken(
            Guid taskKey,
            Guid postKey,
            Func<Guid, PostProcessStatus, CancellationToken, ValueTask>? callback)
        {
            TaskKey = taskKey;
            PostKey = postKey;
            this.callback = callback;
        }

        static BackgroundTaskToken()
        {
            Empty = new BackgroundTaskToken(Guid.Empty, Guid.Empty, null);
        }

        public ValueTask DisposeAsync() => DisposeAsync(true);

        public ValueTask SuccessAsync(CancellationToken cancellationToken)
            => CallbackInvokeAsync(PostProcessStatus.Success, cancellationToken);

        public ValueTask SkipAsync(CancellationToken cancellationToken)
            => CallbackInvokeAsync(PostProcessStatus.Skipped, cancellationToken);

        private async ValueTask CallbackInvokeAsync(PostProcessStatus status, CancellationToken cancellationToken)
        {
            if (null != callback)
            {
                await callback.Invoke(TaskKey, status, cancellationToken);
                callback = null;
            }
        }

        private async ValueTask DisposeAsync(bool dispose)
        {
            if (disposed)
            {
                return ;
            }

            try
            {
                if (dispose)
                {
                    await CallbackInvokeAsync(PostProcessStatus.Failed, CancellationToken.None);
                }
            }
            finally
            {
                disposed = true;
            }
        }
    }

    #endregion
}