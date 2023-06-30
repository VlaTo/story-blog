using Microsoft.Extensions.Logging;
using StoryBlog.Web.Hub.Common.Messages;
using StoryBlog.Web.Microservices.Posts.Shared.Messages;

namespace StoryBlog.Web.Microservices.Posts.Application.MessageHub.Handlers;

internal sealed class NewPostPublishedMessageHandler : IHubMessageHandler<NewPostPublishedMessage>
{
    private readonly ILogger<NewPostPublishedMessageHandler> logger;

    public NewPostPublishedMessageHandler(ILogger<NewPostPublishedMessageHandler> logger)
    {
        this.logger = logger;
    }

    public Task HandleAsync(NewPostPublishedMessage message, CancellationToken cancellationToken)
    {
        logger.LogDebug($"Post: {message.PostKey} slug: {message.Slug}");
        return Task.CompletedTask;
    }
}