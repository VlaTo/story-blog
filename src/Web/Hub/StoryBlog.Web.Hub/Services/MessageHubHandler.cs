using System.Net.WebSockets;
using StoryBlog.Web.Hub.Common.Services;

namespace StoryBlog.Web.Hub.Services;

internal sealed class MessageHubHandler : IDisposable
{
    private bool disposed;
    private readonly MessageHubService hubService;
    private readonly IMessageSerializer serializer;

    public WebSocket WebSocket
    {
        get;
    }

    public MessageHubHandler(
        WebSocket webSocket,
        MessageHubService hubService,
        IMessageSerializer serializer)
    {
        WebSocket = webSocket;

        this.hubService = hubService;
        this.serializer = serializer;
    }

    public async Task HandleAsync(CancellationToken cancellationToken = default)
    {
        if (WebSocket is not { State: WebSocketState.Open })
        {
            await ReceiveMessageAsync(
                message => { },
                cancellationToken
            );
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }

    private async Task ReceiveMessageAsync(Action<Memory<byte>> action, CancellationToken cancellationToken)
    {
        var message = new byte[0];
        var buffer = new byte[128];
        var memory = new Memory<byte>(buffer);

        while (WebSocket is { State: WebSocketState.Open })
        {
            var result = await WebSocket.ReceiveAsync(memory, cancellationToken);

            if (WebSocketMessageType.Close == result.MessageType)
            {
                //webSocket.CloseStatus
                break;
            }

            if (WebSocketMessageType.Binary == result.MessageType)
            {
                if (0 < result.Count)
                {
                    var position = message.Length;

                    Array.Resize(ref message, message.Length + result.Count);
                    Array.Copy(buffer, 0, message, position, result.Count);
                }

                if (result.EndOfMessage)
                {
                    action.Invoke(message);
                }

                continue;
            }

            throw new NotSupportedException();
        }
    }

    private async Task SendMessageAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(Timeout.Infinite, cancellationToken);
    }

    private void Dispose(bool dispose)
    {
        if (disposed)
        {
            return;
        }

        try
        {
            if (dispose)
            {
                hubService.RemoveSocketHandler(this);
            }
        }
        finally
        {
            disposed = true;
        }
    }
}