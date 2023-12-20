using StoryBlog.Web.Microservices.Posts.Shared.Models;

namespace StoryBlog.Web.Client.Blog.Models;

internal sealed record BriefPostModel(
    Guid Key,
    string Slug,
    string Title,
    string Author,
    PostPublicationStatus PublicationStatus,
    string Text,
    AllowedActions AllowedActions,
    PostState State,
    PostVisibilityStatus VisibilityStatus,
    long CommentsCount,
    DateTimeOffset CreatedAt
);