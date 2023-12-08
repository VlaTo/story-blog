using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Posts.Application.Models;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;
using StoryBlog.Web.Microservices.Posts.Domain.Specifications;
using System.Threading.Channels;
using StoryBlog.Web.Common.Result.Extensions;

namespace StoryBlog.Web.Microservices.Posts.Application.Services;

internal sealed class BlogPostProcessingBackgroundService : BackgroundService
{

    private readonly IServiceProvider serviceProvider;
    private readonly IBlogPostProcessingQueue queue;
    private readonly ILogger<BlogPostProcessingBackgroundService> logger;

    public BlogPostProcessingBackgroundService(
        IServiceProvider serviceProvider,
        IBlogPostProcessingQueue queue,
        ILogger<BlogPostProcessingBackgroundService> logger)
    {
        this.serviceProvider = serviceProvider;
        this.queue = queue;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogDebug("Starting background processing");

        try
        {
            // queue persisted backgroundTask from store
            await QueuePendingTasksAsync(cancellationToken: stoppingToken);

            // main propessing loop
            while (true)
            {
                logger.LogDebug("Ready to process");

                var result = await queue.DequeueTaskAsync(cancellationToken: stoppingToken);

                if (result.Failed())
                {
                    throw result.Error!;
                }

                if (result.IsOfT2)
                {
                    break;
                }

                var backgroundTask = result.Item1!;

                await using (var scope = serviceProvider.CreateAsyncScope())
                {
                    await using (var token = await StartProcessingAsync(scope.ServiceProvider, backgroundTask, cancellationToken: stoppingToken))
                    {
                        var service = ActivatorUtilities.CreateInstance<BlogPostProcessingService>(scope.ServiceProvider);

                        await service.ProcessAsync(backgroundTask, stoppingToken);
                        await token.UpdateStatusAsync(PostProcessStatus.Success, cancellationToken: stoppingToken);
                    }
                }
            }
        }
        finally
        {
            logger.LogDebug("Background processing completed");
        }
    }

    private async Task<ProcessingToken> StartProcessingAsync(
        IServiceProvider provider,
        IBackgroundTask backgroundTask,
        CancellationToken cancellationToken)
    {
        var token = new ProcessingToken(provider, backgroundTask.TaskKey);

        await token.UpdateStatusAsync(PostProcessStatus.Processing, cancellationToken);

        return token;
    }

    private async Task QueuePendingTasksAsync(CancellationToken cancellationToken)
    {
        await using (var scope = serviceProvider.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<IAsyncUnitOfWork>();

            await using (var repository = context.GetRepository<PostProcessTask>())
            {
                var specification = new AllPendingPostProcessingTasksSpecification();
                var postProcessingTasks = await repository.QueryAsync(specification, cancellationToken);

                foreach (var task in postProcessingTasks)
                {
                    await queue.EnqueueTaskAsync(task.Key, task.PostKey, cancellationToken);
                }
            }
        }
    }

    #region class: ProcessingToken

    private sealed class ProcessingToken : IAsyncDisposable
    {
        private readonly IServiceProvider serviceProvider;
        private readonly Guid taskKey;

        public ProcessingToken(IServiceProvider serviceProvider, Guid taskKey)
        {
            this.serviceProvider = serviceProvider;
            this.taskKey = taskKey;
        }

        public async ValueTask UpdateStatusAsync(PostProcessStatus status, CancellationToken cancellationToken)
        {
            var context = serviceProvider.GetRequiredService<IAsyncUnitOfWork>();

            await using (var repository = context.GetRepository<PostProcessTask>())
            {
                var specification = new FindTaskByKeySpecification(taskKey);
                var entity = await repository.FindAsync(specification, cancellationToken);

                if (null == entity)
                {
                    return ;
                }

                entity.Status = status;

                if (PostProcessStatus.Pending != status && PostProcessStatus.Processing != status)
                {
                    entity.Completed = DateTimeOffset.Now;
                }

                await repository.SaveChangesAsync(cancellationToken);
            }
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }
    }

    #endregion
}