namespace StoryBlog.Web.Common.Events;

public enum BlogPostAction : byte
{
    Submitted,
    Authored,
    Updated,
    Deleted
}

/// <summary>
/// 
/// </summary>
/// <param name="Key"></param>
public sealed record NewPostCreatedEvent(
    Guid Key,
    DateTimeOffset Created,
    string? AuthorId
);

public sealed record BlogPostEvent(Guid Key, BlogPostAction Action);