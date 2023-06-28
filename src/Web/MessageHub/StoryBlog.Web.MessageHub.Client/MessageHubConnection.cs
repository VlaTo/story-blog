using System.Net.WebSockets;
using StoryBlog.Web.Hub.Common.Messages;
using StoryBlog.Web.MessageHub.Services;

namespace StoryBlog.Web.MessageHub.Client;

public class MessageHubConnection : WebSocketTransport
{
    private readonly Uri hubUri;
    private readonly IHubMessageSerializer serializer;
    private readonly Dictionary<string, MessageHandler> handlers;

    public MessageHubConnection(Uri hubUri, IHubMessageSerializer serializer)
    {
        handlers = new Dictionary<string, MessageHandler>();
        this.hubUri = hubUri;
        this.serializer = serializer;
    }

    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        var webSocket = new ClientWebSocket();

        await webSocket.ConnectAsync(hubUri, cancellationToken);

        if (WebSocketState.Open != webSocket.State)
        {
            return;
        }

        WebSocket = webSocket;

        await ReceiveAsync(HandleMessage, cancellationToken);
    }

    public MessageHubConnection On<TMessage>(string channel, Func<TMessage, Task> handler)
        where TMessage : IHubMessage
    {
        //var key = typeof(TMessage).FullName!;

        if (handlers.ContainsKey(channel))
        {
            throw new Exception();
        }

        var messageHandler = new MessageHandler<TMessage>(channel, handler);

        handlers.Add(channel, messageHandler);

        return this;
    }

    private async Task HandleMessage(ArraySegment<byte> data)
    {
        //ConsoleHelper.PrintBuffer(data);

        var message = Message.From(data);

        if (false == handlers.TryGetValue(message.Channel, out var handler))
        {
            throw new Exception();
        }

        var hubMessage = serializer.Deserialize(handler.MessageType, message.Payload);

        await handler.HandleAsync(hubMessage);
    }
}