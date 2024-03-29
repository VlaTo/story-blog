using AutoMapper;
using MediatR;
using SlimMessageBus;
using StoryBlog.Web.Microservices.Communication.Application.Handlers.NewPostCreated;
using StoryBlog.Web.Microservices.Posts.Events;

namespace StoryBlog.Web.Microservices.Communication.WebApi.MessageBus.Consumers;

public class NewPostCreatedEventConsumer : MessageConsumer, IConsumer<NewPostCreatedEvent>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IHttpContextAccessor contextAccessor;

    public NewPostCreatedEventConsumer(
        IMediator mediator,
        IMapper mapper,
        IHttpContextAccessor contextAccessor,
        ILogger<NewPostCreatedEventConsumer> logger)
        : base(logger)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.contextAccessor = contextAccessor;
    }

    public async Task OnHandle(NewPostCreatedEvent message)
    {
        var cancellationToken = contextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;
        var command = mapper.Map<NewPostCreatedCommand>(message);

        await mediator.Publish(command, cancellationToken);
        
        Logger.LogError("StoryBlog.Web.Microservices.Communication.WebApi.MessageBus.Consumers.NewPostCreatedMessageConsumer");
    }
}