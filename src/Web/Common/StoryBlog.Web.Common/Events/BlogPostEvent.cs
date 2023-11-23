namespace StoryBlog.Web.Common.Events;

/// <summary>
/// 
/// </summary>
public sealed class NewPostCreatedEvent
{
    public Guid Key
    {
        get;
        set;
    }

    public DateTimeOffset Created
    {
        get; 
        set;
    }

    public string? AuthorId
    {
        get; 
        set;
    }
}

/// <summary>
/// 
/// </summary>
/// <param name="Key"></param>
public sealed record PostPublishedEvent(
    Guid Key
);

/// <summary>
/// 
/// </summary>
/// <param name="Key"></param>
public sealed record PostRemovedEvent(
    Guid Key,
    DateTimeOffset Created,
    string? AuthorId
);
