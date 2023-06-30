using StoryBlog.Web.MessageHub.Server.Configuration;
using StoryBlog.Web.Microservices.Posts.Application.MessageHub.Handlers;
using StoryBlog.Web.Microservices.Posts.Shared.Messages;

namespace StoryBlog.Web.Microservices.Posts.Application.Extensions;

public static class MessageHubChannelBuilderExtensions
{
    public static MessageHubChannelBuilder AddHubMessageHandlers(this MessageHubChannelBuilder channelBuilder)
    {
        channelBuilder
            .AddMessage<NewPostPublishedMessage>()
            .WithHandler<NewPostPublishedMessageHandler>();

        return channelBuilder;
    }
}