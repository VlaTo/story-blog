namespace StoryBlog.Web.Common.Events;

/// <summary>
/// 
/// </summary>
/// <param name="Key"></param>
public sealed record NewPostCreatedEvent(
    Guid Key,
    DateTimeOffset Created,
    string? AuthorId
);

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
