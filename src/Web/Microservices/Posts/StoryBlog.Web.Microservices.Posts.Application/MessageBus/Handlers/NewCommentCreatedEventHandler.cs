using Microsoft.Extensions.Logging;
using SlimMessageBus;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Events;
using StoryBlog.Web.Microservices.Posts.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Posts.Application.MessageBus.Handlers;

public sealed class NewCommentCreatedEventHandler : IConsumer<NewCommentCreatedEvent>
{
    private readonly IAsyncUnitOfWork context;
    private readonly ILogger<NewCommentCreatedEventHandler> logger;

    public NewCommentCreatedEventHandler(
        IAsyncUnitOfWork context,
        ILogger<NewCommentCreatedEventHandler> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task OnHandle(NewCommentCreatedEvent message)
    {
        logger.LogInformation($"New Comment Created received event for key: \"{message.Key}\"");

        if (false == await UpdateCommentsCountAsync(message.PostKey, 1))
        {
            logger.LogWarning($"No post found for key: {message.PostKey}");
        }
    }

    private async Task<bool> UpdateCommentsCountAsync(Guid key, int delta)
    {
        await using (var repository = context.GetRepository<Domain.Entities.Post>())
        {
            var post = await repository.FindAsync(new FindPostByKeySpecification(key));

            if (null == post)
            {
                return false;
            }

            post.CommentsCounter.Counter += delta;

            await repository.SaveChangesAsync();
        }

        return true;
    }
}