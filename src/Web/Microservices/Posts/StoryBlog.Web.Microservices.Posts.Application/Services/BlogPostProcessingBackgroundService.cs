using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Common.Result.Extensions;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;
using StoryBlog.Web.Microservices.Posts.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Posts.Application.Services;

internal sealed class BlogPostProcessingBackgroundService : BackgroundService
{
    private readonly IServiceProvider serviceProvider;
    private readonly IBlogPostProcessingManager processingManager;
    private readonly ILogger<BlogPostProcessingBackgroundService> logger;

    public BlogPostProcessingBackgroundService(
        IServiceProvider serviceProvider,
        IBlogPostProcessingManager processingManager,
        ILogger<BlogPostProcessingBackgroundService> logger)
    {
        this.serviceProvider = serviceProvider;
        this.processingManager = processingManager;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogDebug("Starting background processing");

        try
        {
            // queue persisted backgroundTask from store
            await processingManager.QueuePendingPostTasksAsync(cancellationToken: stoppingToken);

            // main propessing loop
            while (true)
            {
                logger.LogDebug("Ready to process");

                var result = await processingManager.ReadPostTaskAsync(stoppingToken);
                    
                if (result.Failed())
                {
                    break;
                }

                await using (var token = await processingManager.StartProcessingAsync(result.Value!, cancellationToken: stoppingToken))
                {
                    var processingResult = await ProcessBlogPostAsync(result.Value!, stoppingToken);

                    if (processingResult.Succeeded)
                    {
                        await token.SuccessAsync(cancellationToken: stoppingToken);
                    }
                    else
                    {
                        await token.SkipAsync(cancellationToken: stoppingToken);
                    }
                }
            }
        }
        finally
        {
            logger.LogDebug("Background processing completed");
        }
    }

    private async Task<IResult> ProcessBlogPostAsync(IBackgroundTask backgroundTask, CancellationToken cancellationToken)
    {
        logger.LogDebug($"Processing task with ID: {backgroundTask.TaskKey:D}");

        await using (var services= serviceProvider.CreateAsyncScope())
        {
            var context = services.ServiceProvider.GetRequiredService<IAsyncUnitOfWork>();

            await using (var repository = context.GetRepository<Post>())
            {
                var specification = new FindPostByKeySpecification(backgroundTask.PostKey, true);
                var post = await repository.FindAsync(specification, cancellationToken);

                if (null == post)
                {
                    return Result.Fail();
                }

                try
                {
                    post.Content.Brief = post.Content.Text;

                    await repository.SaveChangesAsync(cancellationToken);

                    return Result.Success;
                }
                catch (Exception exception)
                {
                    return Result.Fail(exception);
                }
            }
        }
    }
}