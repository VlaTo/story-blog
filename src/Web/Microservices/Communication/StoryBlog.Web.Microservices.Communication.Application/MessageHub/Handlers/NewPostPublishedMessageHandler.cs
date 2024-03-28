using Microsoft.Extensions.Logging;
using StoryBlog.Web.MessageHub.Messages;
using StoryBlog.Web.Microservices.Communication.Shared.Messages;

namespace StoryBlog.Web.Microservices.Communication.Application.MessageHub.Handlers;

public sealed class NewPostPublishedMessageHandler : IHubMessageHandler<NewPostPublishedMessage>
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