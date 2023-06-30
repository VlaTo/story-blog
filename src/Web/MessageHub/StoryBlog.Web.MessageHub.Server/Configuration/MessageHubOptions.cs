using StoryBlog.Web.MessageHub.Services;

namespace StoryBlog.Web.MessageHub.Server.Configuration;

public sealed class MessageHubOptions
{
    public Dictionary<string, MessageHubChannel> Channels
    {
        get;
        private set;
    }

    public string Path
    {
        get;
        set;
    }

    public IHubMessageSerializer Serializer
    {
        get;
        set;
    }

    public MessageHubOptions()
    {
        Channels = new Dictionary<string, MessageHubChannel>();
    }

    public MessageHubOptions Channel(string channel, Action<MessageHubChannelBuilder> channelAction)
    {
        var channelBuilder = new MessageHubChannelBuilder(channel);

        channelAction.Invoke(channelBuilder);

        Channels.Add(
            channel,
            ((IBuilder<MessageHubChannel>)channelBuilder).Build()
        );

        return this;
    }
}