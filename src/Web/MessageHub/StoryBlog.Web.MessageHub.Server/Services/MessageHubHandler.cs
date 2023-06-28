using System.Net.WebSockets;
using StoryBlog.Web.Hub.Common.Messages;
using StoryBlog.Web.MessageHub.Services;

namespace StoryBlog.Web.MessageHub.Server.Services;

internal sealed class MessageHubHandler : WebSocketTransport
{
    private readonly IServiceProvider serviceProvider;
    private readonly MessageHubService hubService;
    private readonly IHubMessageSerializer serializer;

    public MessageHubHandler(
        IServiceProvider serviceProvider,
        WebSocket webSocket,
        MessageHubService hubService,
        IHubMessageSerializer serializer)
    {
        WebSocket = webSocket;

        this.serviceProvider = serviceProvider;
        this.hubService = hubService;
        this.serializer = serializer;
    }

    public async Task HandleAsync(CancellationToken cancellationToken = default)
    {
        if (WebSocket is not { State: WebSocketState.Open })
        {
            throw new Exception();
        }

        await ReceiveAsync(HandleMessage, cancellationToken);
    }

    public async Task SendAsync(ArraySegment<byte> message, CancellationToken cancellationToken = default)
    {
        await WebSocket.SendAsync(message, WebSocketMessageType.Binary, true, cancellationToken);
    }

    private async Task HandleMessage(ArraySegment<byte> data)
    {
        var message = Message.From(data);

        //hubService.
    }
}