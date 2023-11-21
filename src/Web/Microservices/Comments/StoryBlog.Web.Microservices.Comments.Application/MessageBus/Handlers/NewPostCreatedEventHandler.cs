using Microsoft.Extensions.Logging;
using SlimMessageBus;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Events;

namespace StoryBlog.Web.Microservices.Comments.Application.MessageBus.Handlers;

public sealed class NewPostCreatedEventHandler : IConsumer<NewPostCreatedEvent>
{
    private readonly IAsyncUnitOfWork context;
    private readonly ILogger<NewPostCreatedEventHandler> logger;

    public NewPostCreatedEventHandler(
        IAsyncUnitOfWork context,
        ILogger<NewPostCreatedEventHandler> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public Task OnHandle(NewPostCreatedEvent message)
    {
        logger.LogInformation($"Received {nameof(NewPostCreatedEvent)} event");

        return Task.CompletedTask;
    }
}