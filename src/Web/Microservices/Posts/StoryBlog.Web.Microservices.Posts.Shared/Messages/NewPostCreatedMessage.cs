namespace StoryBlog.Web.Microservices.Posts.Shared.Messages;

public sealed record NewPostCreatedMessage(
    Guid PostKey,
    DateTimeOffset Created,
    string AuthorId
);