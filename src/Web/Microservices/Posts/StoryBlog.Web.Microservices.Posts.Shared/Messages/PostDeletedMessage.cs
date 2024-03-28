namespace StoryBlog.Web.Microservices.Posts.Shared.Messages;

public sealed record PostDeletedMessage(
    Guid PostKey
);