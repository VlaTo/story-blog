using AutoMapper;
using MediatR;
using SlimMessageBus;
using StoryBlog.Web.Microservices.Comments.Application.Handlers.PostPublished;
using StoryBlog.Web.Microservices.Posts.Events;

namespace StoryBlog.Web.Microservices.Comments.WebApi.MessageBus.Consumers;

internal sealed class PostPublishedEventConsumer : IConsumer<PostPublishedEvent>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IHttpContextAccessor contextAccessor;
    private readonly ILogger<PostPublishedEventConsumer> logger;

    public PostPublishedEventConsumer(
        IMediator mediator,
        IMapper mapper,
        IHttpContextAccessor contextAccessor,
        ILogger<PostPublishedEventConsumer> logger)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.contextAccessor = contextAccessor;
        this.logger = logger;
    }

    public Task OnHandle(PostPublishedEvent message)
    {
        logger.LogDebug("StoryBlog.Web.Microservices.Comments.WebApi.MessageBus.Consumers.PostPublishedEventConsumer");

        var command = mapper.Map<PostPublishedCommand>(message);
        var cancellationToken = contextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;

        return mediator.Publish(command, cancellationToken);
    }
}