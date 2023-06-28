using StoryBlog.Web.Hub.Common.Messages;

namespace StoryBlog.Web.MessageHub.Configuration;

public class HubMessageHandlerBuilder<TMessage>
    where TMessage : IHubMessage
{
    public HubMessageHandlerBuilder<TMessage> WithHandler<THandler>()
        where THandler : IHubMessageHandler<TMessage>
    {
        var type = typeof(THandler);

        return this;
    }
}