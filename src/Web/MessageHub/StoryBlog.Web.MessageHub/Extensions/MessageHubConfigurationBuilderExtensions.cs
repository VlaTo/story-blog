using StoryBlog.Web.Hub.Common.Messages;
using StoryBlog.Web.MessageHub.Configuration;

namespace StoryBlog.Web.MessageHub.Extensions;

public static class MessageHubConfigurationBuilderExtensions
{
    public static HubMessageHandlerBuilder<TMessage> AddMessage<TMessage>(this MessageHubConfigurationBuilder builder)
        where TMessage : IHubMessage
    {
        var handlerBuilder = new HubMessageHandlerBuilder<TMessage>();
        return handlerBuilder;
    }
}