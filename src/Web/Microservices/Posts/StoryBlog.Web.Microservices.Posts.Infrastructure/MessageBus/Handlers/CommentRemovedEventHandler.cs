using Microsoft.Extensions.Logging;
using SlimMessageBus;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Events;

namespace StoryBlog.Web.Microservices.Posts.Infrastructure.MessageBus.Handlers;

public sealed class CommentRemovedEventHandler : IConsumer<CommentRemovedEvent>
{
    private readonly IAsyncUnitOfWork context;
    private readonly ILogger<CommentRemovedEventHandler> logger;

    public CommentRemovedEventHandler(
        IAsyncUnitOfWork context,
        ILogger<CommentRemovedEventHandler> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public Task OnHandle(CommentRemovedEvent message)
    {
        logger.LogInformation($"Comment Removed event received for key: \"{message.Key}\"");
        return Task.CompletedTask;
    }
}