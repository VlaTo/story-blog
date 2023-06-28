using StoryBlog.Web.MessageHub.Configuration;

namespace StoryBlog.Web.MessageHub.Server;

internal sealed class MessageHubOptionBuilder : IMessageHubOptionBuilder
{
    public MessageHubOptionBuilder()
    {
    }

    public MessageHubOptions Build()
    {
        return new MessageHubOptions();
    }
}