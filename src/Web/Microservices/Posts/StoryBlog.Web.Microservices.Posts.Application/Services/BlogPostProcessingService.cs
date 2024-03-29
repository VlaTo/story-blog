using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;
using StoryBlog.Web.Microservices.Posts.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Posts.Application.Services;

internal sealed class BlogPostProcessingService : IProcessor<IBackgroundTask>
{
    private readonly IAsyncUnitOfWork context;
    private readonly IMessageBusNotification notification;
    private readonly ILogger<BlogPostProcessingService> logger;

    public BlogPostProcessingService(
        IAsyncUnitOfWork context,
        IMessageBusNotification notification,
        ILogger<BlogPostProcessingService> logger)
    {
        this.context = context;
        this.notification = notification;
        this.logger = logger;
    }

    public async Task ProcessAsync(IBackgroundTask backgroundTask, CancellationToken cancellationToken)
    {
        logger.LogDebug($"Running task: '{backgroundTask.TaskKey:B}' for post: '{backgroundTask.PostKey:B}'");

        await using (var repository = context.GetRepository<Post>())
        {
            var specification = new FindPostByKeySpecification(backgroundTask.PostKey, false);
            var post = await repository.FindAsync(specification, cancellationToken);

            if (null == post)
            {
                logger.LogDebug($"No post with key: {backgroundTask.PostKey:B}'");
                return;
            }

            post.Content.Brief = post.Content.Text;

            await repository.SaveChangesAsync(cancellationToken);
        }

        await notification.PublishPostProcessedAsync(backgroundTask.PostKey, cancellationToken);
    }
}