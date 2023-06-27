using StoryBlog.Web.Hub.Common.Configuration;
using StoryBlog.Web.Hub.Common.Messages;

namespace StoryBlog.Web.Hub.Common.Extensions;

public static class MessageHubConfigurationBuilderExtensions
{
    public static HubMessageHandlerBuilder<TMessage> AddMessage<TMessage>(this MessageHubConfigurationBuilder builder)
        where TMessage : IHubMessage
    {
        var handlerBuilder = new HubMessageHandlerBuilder<TMessage>();
        return handlerBuilder;
    }
}