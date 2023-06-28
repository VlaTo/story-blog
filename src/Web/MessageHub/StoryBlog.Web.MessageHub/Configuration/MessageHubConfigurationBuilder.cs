using StoryBlog.Web.Hub.Common.Messages;

namespace StoryBlog.Web.MessageHub.Configuration;

public class MessageHubConfigurationBuilder
{
    public MessageHubConfigurationBuilder()
    {
    }

    public HubMessageHandlerBuilder<TMessage> AddMessage<TMessage>()
        where TMessage : IHubMessage
    {
        var messageBuilder = new HubMessageHandlerBuilder<TMessage>();
        return messageBuilder;
    }

    public MessageHubConfiguration Build()
    {
        return new MessageHubConfiguration();
    }
}