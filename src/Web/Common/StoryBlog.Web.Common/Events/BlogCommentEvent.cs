namespace StoryBlog.Web.Common.Events;

/// <summary>
/// 
/// </summary>
/// <param name="Key"></param>
/// <param name="PostKey"></param>
/// <param name="ParentKey"></param>
/// <param name="CreateAt"></param>
/// <param name="Author"></param>
public sealed record NewCommentCreatedEvent(
    Guid Key,
    Guid PostKey,
    Guid? ParentKey,
    string? Author,
    int ApprovedCommentsCount
);

/// <summary>
/// 
/// </summary>
/// <param name="Key"></param>
/// <param name="PostKey"></param>
/// <param name="ParentKey"></param>
/// <param name="CreateAt"></param>
/// <param name="Author"></param>
public sealed record CommentPublishedEvent(
    Guid Key,
    Guid PostKey,
    Guid? ParentKey,
    DateTimeOffset CreateAt,
    string? Author
);

/// <summary>
/// 
/// </summary>
/// <param name="Key"></param>
/// <param name="PostKey"></param>
/// <param name="ParentKey"></param>
/// <param name="DeletedAt"></param>
/// <param name="AuthorId"></param>
public sealed record CommentRemovedEvent(
    Guid Key,
    Guid PostKey,
    Guid? ParentKey,
    DateTimeOffset DeletedAt,
    string? AuthorId
);