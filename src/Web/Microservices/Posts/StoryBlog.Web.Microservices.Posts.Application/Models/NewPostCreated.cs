namespace StoryBlog.Web.Microservices.Posts.Application.Models;

public sealed record NewPostCreated(
    long Id,
    Guid Key,
    string Slug,
    DateTimeOffset CreatedAt,
    string AuthorId
);