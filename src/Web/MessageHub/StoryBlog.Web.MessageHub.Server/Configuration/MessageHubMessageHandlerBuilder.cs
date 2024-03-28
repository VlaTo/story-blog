using StoryBlog.Web.MessageHub.Messages;

namespace StoryBlog.Web.MessageHub.Server.Configuration;

public class MessageHubMessageHandlerBuilder<TMessage>
    where TMessage : IHubMessage
{
    private readonly List<MessageHubMessage> messages;

    public MessageHubMessageHandlerBuilder(List<MessageHubMessage> messages)
    {
        this.messages = messages;
    }

    public MessageHubMessageHandlerBuilder<TMessage> WithHandler<THandler>()
        where THandler : IHubMessageHandler<TMessage>
    {
        var messageType = typeof(TMessage);
        var message = messages.Find(x => x.MessageType == messageType);

        if (null == message)
        {
            message = new MessageHubMessage(messageType);
            messages.Add(message);
        }

        message.Handlers.Add(typeof(THandler));

        return this;
    }
}