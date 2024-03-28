using Microsoft.Extensions.Options;
using SlimMessageBus;
using StoryBlog.Web.Microservices.Posts.Application.Services;
using StoryBlog.Web.Microservices.Posts.Shared.Messages;
using StoryBlog.Web.Microservices.Posts.WebApi.Configuration;
using StoryBlog.Web.Microservices.Posts.WebApi.Extensions;

namespace StoryBlog.Web.Microservices.Posts.WebApi.Services;

internal sealed class SlimMessageBusNotification : IMessageBusNotification
{
    private readonly IMessageBus messageBus;
    private readonly MessageBusOptions options;
    private readonly ILogger<SlimMessageBusNotification> logger;

    public SlimMessageBusNotification(
        IMessageBus messageBus,
        IOptions<MessageBusOptions> options,
        ILogger<SlimMessageBusNotification> logger)
    {
        this.messageBus = messageBus;
        this.options = options.Value;
        this.logger = logger;
    }

    public async Task NewPostCreatedAsync(
    Guid postKey,
        DateTimeOffset createdAt,
        string authorId,
        CancellationToken cancellationToken)
    {
        if (false == options.PublishCreatedEvent)
        {
            logger.LogPostCreatedNotificationDisabled(postKey, authorId);
            return;
        }

        var createdEvent = new NewPostCreatedMessage(postKey, createdAt, authorId);

        await messageBus.Publish(createdEvent, cancellationToken: cancellationToken);

        logger.LogPostCreateNotificationSent(postKey, createdAt, authorId);
    }

    public async Task PostDeletedAsync(
        Guid postKey,
        string authorId,
        CancellationToken cancellationToken)
    {
        if (false == options.PublishRemovedEvent)
        {
            logger.LogPostDeletedNotificationDisabled(postKey);
            return;
        }

        var deletedMessage = new PostDeletedMessage(postKey);
        
        await messageBus.Publish(deletedMessage, cancellationToken: cancellationToken);
        
        logger.LogPostDeletedNotificationSent(postKey, authorId);
    }
}