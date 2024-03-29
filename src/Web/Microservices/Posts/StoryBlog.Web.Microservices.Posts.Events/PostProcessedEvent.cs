namespace StoryBlog.Web.Microservices.Posts.Events;

public sealed class PostProcessedEvent
{
    public required Guid PostKey
    {
        get;
        set;
    }
}