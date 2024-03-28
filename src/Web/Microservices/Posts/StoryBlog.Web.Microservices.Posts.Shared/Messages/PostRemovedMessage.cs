using StoryBlog.Web.MessageHub.Messages;

namespace StoryBlog.Web.Microservices.Posts.Shared.Messages;

public sealed record PostRemovedMessage(
    Guid PostKey,
    string Slug
) : IHubMessage;