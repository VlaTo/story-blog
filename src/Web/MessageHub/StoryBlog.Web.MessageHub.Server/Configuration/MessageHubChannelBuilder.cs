using StoryBlog.Web.Hub.Common.Messages;

namespace StoryBlog.Web.MessageHub.Server.Configuration;

public sealed class MessageHubChannelBuilder : IBuilder<MessageHubChannel>
{
    private readonly List<MessageHubMessage> messages;
    private readonly string channel;

    public MessageHubChannelBuilder(string channel)
    {
        this.channel = channel;
        messages = new List<MessageHubMessage>();
    }

    public MessageHubMessageHandlerBuilder<TMessage> AddMessage<TMessage>()
        where TMessage : IHubMessage
    {
        var messageBuilder = new MessageHubMessageHandlerBuilder<TMessage>(messages);

        return messageBuilder;
    }

    MessageHubChannel IBuilder<MessageHubChannel>.Build()
    {
        return new MessageHubChannel(channel, messages);
    }
}