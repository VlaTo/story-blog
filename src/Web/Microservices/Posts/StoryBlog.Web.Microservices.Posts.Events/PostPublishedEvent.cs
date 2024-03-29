namespace StoryBlog.Web.Microservices.Posts.Events;

public sealed record PostPublishedEvent
{
    public required Guid PostKey
    {
        get;
        set;
    }

    public required string Slug
    {
        get;
        set;
    }
}