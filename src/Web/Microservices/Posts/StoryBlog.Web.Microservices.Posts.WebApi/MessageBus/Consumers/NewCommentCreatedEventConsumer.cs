using AutoMapper;
using MediatR;
using SlimMessageBus;
using StoryBlog.Web.Microservices.Comments.Events;
using StoryBlog.Web.Microservices.Posts.Application.Handlers.NewCommentCreated;

namespace StoryBlog.Web.Microservices.Posts.WebApi.MessageBus.Consumers;

internal sealed class NewCommentCreatedEventConsumer : MessageConsumer, IConsumer<NewCommentCreatedEvent>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IHttpContextAccessor contextAccessor;
    private readonly ILogger<NewCommentCreatedEventConsumer> logger;

    public NewCommentCreatedEventConsumer(
        IMediator mediator,
        IMapper mapper,
        IHttpContextAccessor contextAccessor,
        ILogger<NewCommentCreatedEventConsumer> logger)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.contextAccessor = contextAccessor;
        this.logger = logger;
    }

    public async Task OnHandle(NewCommentCreatedEvent message)
    {
        var command = mapper.Map<NewCommentCreatedCommand>(message);
        var cancellationToken = contextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;

        await mediator.Publish(command, cancellationToken);
    }
}