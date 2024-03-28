using System.Net.WebSockets;
using System.Text;
using StoryBlog.Web.MessageHub.Messages;
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
        if (handlers.ContainsKey(channel))
        {
            throw new Exception();
        }

        var messageHandler = new MessageHandler<TMessage>(channel, handler);

        handlers.Add(channel, messageHandler);

        return this;
    }

    public async Task SendMessageAsync<TMessage>(string channel, TMessage message, CancellationToken cancellationToken = default)
        where TMessage : IHubMessage
    {
        var json = System.Text.Json.JsonSerializer.Serialize(message);
        var temp = new Message(channel, Encoding.UTF8.GetBytes(json));

        await WebSocket.SendAsync(temp.ToBytes(), WebSocketMessageType.Binary, true, cancellationToken);
    }

    private async Task HandleMessage(ArraySegment<byte> data)
    {
        var message = Message.From(data);

        if (false == handlers.TryGetValue(message.Channel, out var handler))
        {
            throw new Exception();
        }

        var hubMessage = serializer.Deserialize(handler.MessageType, message.Payload);

        await handler.HandleAsync(hubMessage);
    }

    protected override void DoDispose()
    {
        ;
    }
}