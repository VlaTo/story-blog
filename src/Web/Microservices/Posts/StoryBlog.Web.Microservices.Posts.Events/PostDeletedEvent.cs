namespace StoryBlog.Web.Microservices.Posts.Events;

public sealed class PostDeletedEvent
{
    public required Guid PostKey
    {
        get;
        set;
    }
}