using Microsoft.Extensions.Logging;
using SlimMessageBus;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Events;

namespace StoryBlog.Web.Microservices.Comments.Application.MessageBus.Handlers;

public sealed class PostPublishedEventHandler : IConsumer<PostPublishedEvent>
{
    private readonly IAsyncUnitOfWork context;
    private readonly ILogger<NewPostCreatedEventHandler> logger;

    public PostPublishedEventHandler(
        IAsyncUnitOfWork context,
        ILogger<NewPostCreatedEventHandler> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public Task OnHandle(PostPublishedEvent message)
    {
        logger.LogInformation($"Received {nameof(PostPublishedEvent)} event");

        return Task.CompletedTask;
    }
}