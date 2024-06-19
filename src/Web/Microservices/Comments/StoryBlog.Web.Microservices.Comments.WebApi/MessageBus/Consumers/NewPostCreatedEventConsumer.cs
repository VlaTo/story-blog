using AutoMapper;
using MediatR;
using SlimMessageBus;
using StoryBlog.Web.Microservices.Comments.Application.Handlers.NewPostCreated;
using StoryBlog.Web.Microservices.Posts.Events;

namespace StoryBlog.Web.Microservices.Comments.WebApi.MessageBus.Consumers;

internal sealed class NewPostCreatedEventConsumer : IConsumer<NewPostCreatedEvent>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IHttpContextAccessor contextAccessor;
    private readonly ILogger<NewPostCreatedEventConsumer> logger;

    public NewPostCreatedEventConsumer(
        IMediator mediator,
        IMapper mapper,
        IHttpContextAccessor contextAccessor,
        ILogger<NewPostCreatedEventConsumer> logger)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.contextAccessor = contextAccessor;
        this.logger = logger;
    }

    public Task OnHandle(NewPostCreatedEvent message)
    {
        logger.LogDebug("StoryBlog.Web.Microservices.Comments.Application.MessageBus.Handlers.NewPostCreatedEventHandler");

        var command = mapper.Map<NewPostCreatedCommand>(message);
        var cancellationToken = contextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;

        return mediator.Publish(command, cancellationToken);
    }
}