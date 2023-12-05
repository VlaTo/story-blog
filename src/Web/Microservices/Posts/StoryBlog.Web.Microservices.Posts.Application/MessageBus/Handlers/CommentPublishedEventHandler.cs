using Microsoft.Extensions.Logging;
using SlimMessageBus;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Events;

namespace StoryBlog.Web.Microservices.Posts.Application.MessageBus.Handlers;

public sealed class CommentPublishedEventHandler : IConsumer<CommentPublishedEvent>
{
    private readonly IAsyncUnitOfWork context;
    private readonly ILogger<CommentPublishedEventHandler> logger;

    public CommentPublishedEventHandler(
        IAsyncUnitOfWork context,
        ILogger<CommentPublishedEventHandler> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public Task OnHandle(CommentPublishedEvent message)
    {
        logger.LogInformation($"Comment Published event received for key: \"{message.Key}\"");
        return Task.CompletedTask;
    }
}