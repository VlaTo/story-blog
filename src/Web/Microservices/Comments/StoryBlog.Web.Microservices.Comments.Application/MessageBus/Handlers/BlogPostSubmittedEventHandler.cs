using Microsoft.Extensions.Logging;
using SlimMessageBus;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Events;
using StoryBlog.Web.Microservices.Comments.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Comments.Application.MessageBus.Handlers;

public sealed class BlogPostSubmittedEventHandler : IConsumer<BlogPostEvent>
{
    private readonly IUnitOfWork context;
    private readonly ILogger<BlogPostSubmittedEventHandler> logger;

    public BlogPostSubmittedEventHandler(IUnitOfWork context, ILogger<BlogPostSubmittedEventHandler> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public Task OnHandle(BlogPostEvent message)
    {
        logger.LogInformation($"Received event: action = {message.Action} for key: \"{message.Key}\"");

        switch (message.Action)
        {
            case BlogPostAction.Deleted:
            {
                return DeletePostCommentsAsync(message.Key);
            }
            default:
            {
                break;
            }
        }

        return Task.CompletedTask;
    }

    private async Task DeletePostCommentsAsync(Guid postKey)
    {
        var dateTime = DateTime.UtcNow;

        await using (var repository = context.GetRepository<Domain.Entities.Comment>())
        {
            var specification = new FindRootCommentsForPostSpecification(postKey, includeAll: false);
            var comments = await repository.QueryAsync(specification, CancellationToken.None);

            if (0 < comments.Length)
            {
                logger.LogWarning($"No comments for post: {postKey}");
                return;
            }

            for (var index = 0; index < comments.Length; index++)
            {
                comments[index].DeletedAt = dateTime;
                logger.LogDebug($"Comment id: {comments[index].Id} for post: {postKey:D} was deleted");
            }

            await repository.SaveChangesAsync(CancellationToken.None);
        }
    }
}