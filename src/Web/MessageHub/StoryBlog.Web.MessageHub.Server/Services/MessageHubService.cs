using System.Net.WebSockets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StoryBlog.Web.Hub.Common.Messages;
using StoryBlog.Web.MessageHub.Configuration;
using StoryBlog.Web.MessageHub.Services;

namespace StoryBlog.Web.MessageHub.Server.Services;

internal sealed class MessageHubService
{
    private readonly MessageHubOptions options;
    private readonly List<(WebSocket, MessageHubHandler)> handlers;
    private readonly IServiceProvider serviceProvider;
    private readonly ILogger<MessageHubService> logger;

    public MessageHubService(
        IServiceProvider serviceProvider,
        IOptions<MessageHubOptions> options,
        ILogger<MessageHubService> logger)
    {
        handlers = new List<(WebSocket, MessageHubHandler)>();
        this.options = options.Value;
        this.serviceProvider = serviceProvider;
        this.logger = logger;
    }

    public Task<MessageHubHandler> CreateWebSocketHandlerAsync(WebSocket webSocket)
    {
        var handler = new MessageHubHandler(serviceProvider, webSocket, this, options.Serializer);

        handlers.Add((webSocket, handler));

        return Task.FromResult(handler);
    }

    public async Task SendMessageAsync<TMessage>(string channel, TMessage message, CancellationToken cancellationToken)
        where TMessage : IHubMessage
    {
        var payload = options.Serializer.Serialize(message);
        var temp = new Message(channel, payload);

        await handlers[0].Item2.SendAsync(temp.ToBytes(), cancellationToken);
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