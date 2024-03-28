namespace StoryBlog.Web.Client.Blog.Configuration;

public sealed class MessageHubOptions
{
    public const string SectionName = "MessageHub";

    public string ConnectionUri
    {
        get;
        set;
    } = String.Empty;
}