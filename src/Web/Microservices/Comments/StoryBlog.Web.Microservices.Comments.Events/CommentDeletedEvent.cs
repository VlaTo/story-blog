namespace StoryBlog.Web.Microservices.Comments.Events;

public sealed record CommentDeletedEvent(
    Guid Key,
    Guid PostKey,
    Guid? ParentKey,
    DateTimeOffset DeletedAt,
    string? AuthorId
);