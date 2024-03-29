using StoryBlog.Web.MessageHub.Services;

namespace StoryBlog.Web.MessageHub.Server.Configuration;

public sealed class MessageHubOptions
{
    public Dictionary<string, MessageHubChannel> Channels
    {
        get;
    } = new();

    public required string Path
    {
        get;
        set;
    }

    public required IHubMessageSerializer Serializer
    {
        get;
        set;
    }

    public MessageHubOptions Channel(string channel, Action<MessageHubChannelBuilder> channelConfiguration)
    {
        var channelBuilder = new MessageHubChannelBuilder(channel);

        channelConfiguration.Invoke(channelBuilder);

        Channels.Add(
            channel,
            ((IBuilder<MessageHubChannel>)channelBuilder).Build()
        );

        return this;
    }
}