using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Result.Extensions;
using StoryBlog.Web.Microservices.Posts.Application.Extensions;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;
using StoryBlog.Web.Microservices.Posts.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Posts.Application.Services;

internal sealed class BlogPostProcessingBackgroundService(
    IServiceProvider serviceProvider,
    IBlogPostProcessingQueue queue,
    ILogger<BlogPostProcessingBackgroundService> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogBackgroundProcessingStarting();

        try
        {
            // queue persisted backgroundTask from store
            await QueuePendingTasksAsync(cancellationToken: stoppingToken);

            // main processing loop
            while (true)
            {
                if (false == queue.HasPendingTasks)
                {
                    logger.LogBackgroundProcessingReady();
                }

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

                logger.LogCreatingServicesScope(backgroundTask);

                await using (var scope = serviceProvider.CreateAsyncScope())
                {
                    await ProcessBackgroundTaskAsync(backgroundTask, scope, stoppingToken);
                }
            }
        }
        finally
        {
            logger.LogBackgroundProcessingCompleted();
        }
    }

    private async Task ProcessBackgroundTaskAsync(IBackgroundTask backgroundTask, AsyncServiceScope scope, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogStartBackgroundTaskProcessing(backgroundTask);

            await using (var token = await StartProcessingAsync(scope.ServiceProvider, backgroundTask, cancellationToken))
            {
                var service = ActivatorUtilities.CreateInstance<BlogPostProcessingService>(scope.ServiceProvider);

                await service.ProcessAsync(backgroundTask, cancellationToken);
                await token.UpdateStatusAsync(PostProcessStatus.Success, cancellationToken: cancellationToken);
            }

            logger.LogBackgroundTaskProcessed(backgroundTask);
        }
        catch (Exception exception)
        {
            var exceptionInfo = System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(exception);
            logger.LogBackgroundTaskProcessingFailed(backgroundTask, exception, exceptionInfo);
        }
        finally
        {
            logger.LogBackgroundTaskProcessingCompleted(backgroundTask);
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