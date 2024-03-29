namespace StoryBlog.Web.Microservices.Posts.Events;

public sealed class NewPostCreatedEvent
{
    public required Guid PostKey
    {
        get; 
        set;
    }

    public required DateTimeOffset Created
    {
        get; 
        set;
    }

    public required string Slug
    {
        get;
        set;
    }

    public required string AuthorId
    {
        get; 
        set;
    }
}