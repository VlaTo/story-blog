using AutoMapper;
using MediatR;
using SlimMessageBus;
using StoryBlog.Web.Microservices.Comments.Application.Handlers.PostDeleted;
using StoryBlog.Web.Microservices.Posts.Events;

namespace StoryBlog.Web.Microservices.Comments.WebApi.MessageBus.Consumers;

internal sealed class PostDeletedEventConsumer : IConsumer<PostDeletedEvent>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IHttpContextAccessor contextAccessor;
    private readonly ILogger<PostDeletedEventConsumer> logger;

    public PostDeletedEventConsumer(
        IMediator mediator,
        IMapper mapper,
        IHttpContextAccessor contextAccessor,
        ILogger<PostDeletedEventConsumer> logger)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.contextAccessor = contextAccessor;
        this.logger = logger;
    }

    public Task OnHandle(PostDeletedEvent message)
    {
        logger.LogDebug("StoryBlog.Web.Microservices.Comments.WebApi.MessageBus.Consumers.PostDeletedEventConsumer");

        var command = mapper.Map<PostDeletedCommand>(message);
        var cancellationToken = contextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;

        return mediator.Publish(command, cancellationToken);
    }
}