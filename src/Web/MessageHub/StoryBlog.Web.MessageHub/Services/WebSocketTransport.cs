using System.Net.WebSockets;

namespace StoryBlog.Web.MessageHub.Services;

public abstract class WebSocketTransport : IDisposable
{
    private bool disposed;

    public WebSocket? WebSocket
    {
        get;
        protected set;
    }

    public void Dispose()
    {
        Dispose(true);
    }

    protected async Task ReceiveAsync(Func<ArraySegment<byte>, Task> handle, CancellationToken cancellationToken)
    {
        if (WebSocketState.Open != WebSocket?.State)
        {
            throw new Exception();
        }

        var offset = 0;
        var done = false;
        var bytes = new byte[1024];

        while (false == done)
        {
            var buffer = new ArraySegment<byte>(bytes, offset, bytes.Length - offset);
            var result = await WebSocket.ReceiveAsync(buffer, cancellationToken);

            switch (result.MessageType)
            {
                case WebSocketMessageType.Close:
                {
                    done = true;

                    break;
                }

                case WebSocketMessageType.Binary:
                {
                    if (0 < result.Count)
                    {
                        offset += result.Count;
                    }

                    if (result.EndOfMessage)
                    {
                        var message = new ArraySegment<byte>(bytes, 0, offset);

                        await handle.Invoke(message);

                        offset = 0;
                    }

                    break;
                }

                case WebSocketMessageType.Text:
                {
                    break;
                }
            }
        }
    }

    protected abstract void DoDispose();

    protected void Dispose(bool dispose)
    {
        if (disposed)
        {
            return;
        }

        try
        {
            if (dispose)
            {
                WebSocket?.Dispose();
                DoDispose();
            }
        }
        finally
        {
            disposed = true;
        }
    }
}