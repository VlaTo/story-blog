using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Microservices.Communication.Application.Models;
using StoryBlog.Web.Microservices.Communication.Application.Services;

namespace StoryBlog.Web.Microservices.Communication.Application.Handlers.NewPostCreated;

public sealed class NewPostCreatedHandler : INotificationHandler<NewPostCreatedCommand>
{
    private readonly IMessageHubNotification notification;
    private readonly IMapper mapper;
    private readonly ILogger<NewPostCreatedHandler> logger;

    public NewPostCreatedHandler(
        IMessageHubNotification notification,
        IMapper mapper,
        ILogger<NewPostCreatedHandler> logger)
    {
        this.notification = notification;
        this.mapper = mapper;
        this.logger = logger;
    }

    public async Task Handle(NewPostCreatedCommand request, CancellationToken cancellationToken)
    {
        var context = mapper.Map<PublishNewPostCreated>(request);
        await notification.PublishNewPostCreatedEventAsync(context, cancellationToken);
    }
}