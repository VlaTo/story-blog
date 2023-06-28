using StoryBlog.Web.MessageHub.Services;

namespace StoryBlog.Web.MessageHub.Configuration;

public sealed class MessageHubOptions
{
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
}