using AutoMapper;
using MediatR;
using SlimMessageBus;
using StoryBlog.Web.Microservices.Comments.Application.Handlers.NewPostCreated;
using StoryBlog.Web.Microservices.Posts.Events;

namespace StoryBlog.Web.Microservices.Comments.WebApi.MessageBus.Consumers;

public sealed class NewPostCreatedMessageConsumer : IConsumer<NewPostCreatedEvent>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IHttpContextAccessor contextAccessor;
    private readonly ILogger<NewPostCreatedMessageConsumer> logger;

    public NewPostCreatedMessageConsumer(
        IMediator mediator,
        IMapper mapper,
        IHttpContextAccessor contextAccessor,
        ILogger<NewPostCreatedMessageConsumer> logger)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.contextAccessor = contextAccessor;
        this.logger = logger;
    }

    public async Task OnHandle(NewPostCreatedEvent message)
    {
        logger.LogDebug("StoryBlog.Web.Microservices.Comments.Application.MessageBus.Handlers.NewPostCreatedEventHandler");

        var command = mapper.Map<NewPostCreatedCommand>(message);
        var cancellationToken = contextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;

        await mediator.Publish(command, cancellationToken);

    }
}