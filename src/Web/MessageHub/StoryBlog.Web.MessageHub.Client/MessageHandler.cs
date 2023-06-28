using StoryBlog.Web.Hub.Common.Messages;

namespace StoryBlog.Web.MessageHub.Client;

internal abstract class MessageHandler
{
    public abstract Type MessageType
    {
        get;
    }
    public abstract Task HandleAsync(IHubMessage message);
}

internal sealed class MessageHandler<TMessage> : MessageHandler
    where TMessage : IHubMessage
{
    private readonly Func<TMessage, Task> handler;

    public string Channel
    {
        get;
    }

    public override Type MessageType => typeof(TMessage);

    public MessageHandler(string channel, Func<TMessage, Task> handler)
    {
        Channel = channel;
        this.handler = handler;
    }

    public override async Task HandleAsync(IHubMessage message)
    {
        await handler.Invoke((TMessage)message);
    }
}