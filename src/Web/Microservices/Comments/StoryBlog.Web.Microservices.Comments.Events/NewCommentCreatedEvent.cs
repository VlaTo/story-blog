namespace StoryBlog.Web.Microservices.Comments.Events;

public sealed record NewCommentCreatedEvent(
    Guid Key,
    Guid PostKey,
    Guid? ParentKey,
    int ApprovedCommentsCount
);