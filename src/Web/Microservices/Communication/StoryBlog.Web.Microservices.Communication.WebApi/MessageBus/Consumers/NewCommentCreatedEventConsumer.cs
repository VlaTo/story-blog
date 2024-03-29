using AutoMapper;
using SlimMessageBus;
using StoryBlog.Web.Microservices.Comments.Events;

namespace StoryBlog.Web.Microservices.Communication.WebApi.MessageBus.Consumers;

internal sealed class NewCommentCreatedEventConsumer : MessageConsumer, IConsumer<NewCommentCreatedEvent>
{
    private readonly IMapper mapper;
    private readonly IHttpContextAccessor contextAccessor;

    public NewCommentCreatedEventConsumer(
        IMapper mapper,
        IHttpContextAccessor contextAccessor,
        ILogger<NewCommentCreatedEventConsumer> logger)
        : base(logger)
    {
        this.mapper = mapper;
        this.contextAccessor = contextAccessor;
    }

    public Task OnHandle(NewCommentCreatedEvent message)
    {
        var cancellationToken = contextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;
        return Task.CompletedTask;
    }
}