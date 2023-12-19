using StoryBlog.Web.Microservices.Posts.Shared.Models;

namespace StoryBlog.Web.Client.Blog.Models;

internal sealed record BriefPostModel(
    Guid Key,
    string Slug,
    string Title,
    string Author,
    PostModelStatus Status,
    string Text,
    AllowedActions AllowedActions,
    PostState State,
    bool IsPublic,
    long CommentsCount,
    DateTimeOffset CreatedAt
);