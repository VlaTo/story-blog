namespace StoryBlog.Web.Microservices.Comments.Application.Models;

/// <summary>
/// 
/// </summary>
/// <param name="Key"></param>
/// <param name="PostKey"></param>
/// <param name="ParentKey"></param>
/// <param name="Author"></param>
public sealed record NewCommentCreated(
    Guid Key,
    Guid PostKey,
    Guid? ParentKey,
    string? Author,
    int ApprovedCommentsCount
);
