namespace StoryBlog.Web.Common.Events;


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
/// <param name="Created"></param>
/// <param name="AuthorId"></param>
public sealed record PostRemovedEvent(
    Guid Key,
    DateTimeOffset Created,
    string? AuthorId
);
