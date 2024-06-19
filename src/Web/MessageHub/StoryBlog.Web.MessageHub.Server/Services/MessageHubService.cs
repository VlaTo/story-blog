using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StoryBlog.Web.MessageHub.Services;
using System.Net.WebSockets;
using StoryBlog.Web.MessageHub.Messages;
using MessageHubOptions = StoryBlog.Web.MessageHub.Server.Configuration.MessageHubOptions;

namespace StoryBlog.Web.MessageHub.Server.Services;

internal sealed class MessageHubService
{
    private readonly MessageHubOptions options;
    private readonly List<MessageHubHandler> handlers;
    private readonly IServiceProvider serviceProvider;
    private readonly ILogger<MessageHubService> logger;

    public MessageHubService(
        IServiceProvider serviceProvider,
        IOptions<MessageHubOptions> options,
        ILogger<MessageHubService> logger)
    {
        handlers = new List<MessageHubHandler>();
        this.options = options.Value;
        this.serviceProvider = serviceProvider;
        this.logger = logger;
    }

    public Task<MessageHubHandler> CreateWebSocketHandlerAsync(WebSocket webSocket)
    {
        var handler = new MessageHubHandler(this, serviceProvider, webSocket, options);

        handlers.Add(handler);

        return Task.FromResult(handler);
    }

    public async Task SendMessageAsync<TMessage>(string channel, TMessage message, CancellationToken cancellationToken)
        where TMessage : IHubMessage
    {
        var payload = options.Serializer.Serialize(message);
        var hubMessage = new Message(channel, payload);
        var data = hubMessage.ToBytes();

        await Task.WhenAll(handlers.Select(x => x.SendAsync(data, cancellationToken)));
    }

    public void RemoveSocketHandler(MessageHubHandler handler)
    {
        for (var index = 0; index < handlers.Count; index++)
        {
            if (ReferenceEquals(handlers[index], handler))
            {
                handlers.RemoveAt(index);
                handler.WebSocket.Dispose();

                return;
            }
        }
    }
}