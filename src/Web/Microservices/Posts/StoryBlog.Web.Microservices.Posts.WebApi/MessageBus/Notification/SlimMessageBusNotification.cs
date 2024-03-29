using AutoMapper;
using Microsoft.Extensions.Options;
using SlimMessageBus;
using StoryBlog.Web.Microservices.Posts.Application.Models;
using StoryBlog.Web.Microservices.Posts.Application.Services;
using StoryBlog.Web.Microservices.Posts.Events;
using StoryBlog.Web.Microservices.Posts.WebApi.Configuration;
using StoryBlog.Web.Microservices.Posts.WebApi.Extensions;

namespace StoryBlog.Web.Microservices.Posts.WebApi.MessageBus.Notification;

internal sealed class SlimMessageBusNotification : IMessageBusNotification
{
    private readonly IMessageBus messageBus;
    private readonly IMapper mapper;
    private readonly MessageBusOptions options;
    private readonly ILogger<SlimMessageBusNotification> logger;

    public SlimMessageBusNotification(
        IMessageBus messageBus,
        IMapper mapper,
        IOptions<MessageBusOptions> options,
        ILogger<SlimMessageBusNotification> logger)
    {
        this.messageBus = messageBus;
        this.mapper = mapper;
        this.options = options.Value;
        this.logger = logger;
    }

    public async Task NewPostCreatedAsync(NewPostCreated postCreated, CancellationToken cancellationToken)
    {
        if (false == options.PublishCreatedEvent)
        {
            logger.LogPostCreatedNotificationDisabled(postCreated.Key, postCreated.AuthorId);
            return;
        }

        var createdEvent = mapper.Map<NewPostCreatedEvent>(postCreated);

        await messageBus.Publish(createdEvent, cancellationToken: cancellationToken);

        logger.LogPostCreateNotificationSent(postCreated.Key, postCreated.CreatedAt, postCreated.AuthorId);
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

        //var deletedMessage = new PostDeletedMessage(postKey);

        //await messageBus.Publish(deletedMessage, cancellationToken: cancellationToken);

        logger.LogPostDeletedNotificationSent(postKey, authorId);
    }

    public async Task PublishPostProcessedAsync(Guid postKey, CancellationToken cancellationToken)
    {
        if (false == options.PublishPostPostProcessedEvent)
        {
            //logger.LogPostDeletedNotificationDisabled(postKey);
            return;
        }

        //var createdEvent = mapper.Map<NewPostCreatedEvent>(postCreated);
        var postProcessedEvent = new PostProcessedEvent
        {
            PostKey = postKey
        };

        await messageBus.Publish(postProcessedEvent, cancellationToken: cancellationToken);

        //logger.LogPostCreateNotificationSent(postCreated.Key, postCreated.CreatedAt, postCreated.AuthorId);
    }
}