using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.WebSockets;
using StoryBlog.Web.Hub.Common.Configuration;
using StoryBlog.Web.Hub.Common.Messages;

namespace StoryBlog.Web.Hub.Services;

internal sealed class MessageHubService
{
    private readonly MessageHubOptions options;
    private readonly List<(WebSocket, MessageHubHandler)> handlers;
    private readonly ILogger<MessageHubService> logger;

    public MessageHubService(
        IOptions<MessageHubOptions> options,
        ILogger<MessageHubService> logger)
    {
        handlers = new List<(WebSocket, MessageHubHandler)>();
        this.options = options.Value;
        this.logger = logger;
    }

    public Task<MessageHubHandler> CreateWebSocketHandlerAsync(WebSocket webSocket)
    {
        var handler = new MessageHubHandler(webSocket, this, options.Serializer);

        handlers.Add((webSocket, handler));

        return Task.FromResult(handler);
    }

    public Task SendMessageAsync<TMessage>(TMessage message, CancellationToken cancellationToken)
        where TMessage : IHubMessage
    {

        return Task.CompletedTask;
    }

    public void RemoveSocketHandler(MessageHubHandler handler)
    {
        for (int index = 0; index < handlers.Count; index++)
        {
            if (ReferenceEquals(handlers[index].Item2, handler))
            {
                handlers.RemoveAt(index);
                handler.WebSocket.Dispose();

                return;
            }
        }
    }
}