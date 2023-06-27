using StoryBlog.Web.Hub.Common.Configuration;

namespace StoryBlog.Web.Hub;

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