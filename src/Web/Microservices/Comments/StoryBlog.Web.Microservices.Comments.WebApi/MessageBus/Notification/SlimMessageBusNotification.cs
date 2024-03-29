using AutoMapper;
using SlimMessageBus;
using StoryBlog.Web.Microservices.Comments.Application.Models;
using StoryBlog.Web.Microservices.Comments.Application.Services;
using StoryBlog.Web.Microservices.Comments.Events;

namespace StoryBlog.Web.Microservices.Comments.WebApi.MessageBus.Notification;

internal sealed class SlimMessageBusNotification : IMessageBusNotification
{
    private readonly IMessageBus messageBus;
    private readonly IMapper mapper;

    public SlimMessageBusNotification(IMessageBus messageBus, IMapper mapper)
    {
        this.messageBus = messageBus;
        this.mapper = mapper;
    }

    public async Task NewCommentCreatedAsync(NewCommentCreated commentCreated, CancellationToken cancellationToken)
    {
        var commentCreatedEvent = mapper.Map<NewCommentCreatedEvent>(commentCreated);
        await messageBus.Publish(commentCreatedEvent, cancellationToken: cancellationToken);
    }
}