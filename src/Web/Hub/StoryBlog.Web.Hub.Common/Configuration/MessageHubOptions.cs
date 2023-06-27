using StoryBlog.Web.Hub.Common.Services;

namespace StoryBlog.Web.Hub.Common.Configuration;

public sealed class MessageHubOptions
{
    public IMessageSerializer Serializer
    {
        get;
        set;
    }
}