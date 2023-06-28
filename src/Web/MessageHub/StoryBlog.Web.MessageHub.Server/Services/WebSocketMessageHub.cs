using Microsoft.Extensions.Logging;
using StoryBlog.Web.Hub.Common.Messages;

namespace StoryBlog.Web.MessageHub.Server.Services;

internal sealed class WebSocketMessageHub : IMessageHub
{
    private readonly MessageHubService hubService;
    private readonly ILogger<WebSocketMessageHub> logger;

    public WebSocketMessageHub(
        MessageHubService hubService,
        ILogger<WebSocketMessageHub> logger)
    {
        this.hubService = hubService;
        this.logger = logger;
    }

    public Task SendAsync<TMessage>(string channel, TMessage message, CancellationToken cancellationToken = default)
        where TMessage : IHubMessage
    {
        logger.LogDebug($"Sending message to channel: {channel}");
        return hubService.SendMessageAsync(channel, message, cancellationToken);
    }
}