using StoryBlog.Web.Hub.Common.Messages;

namespace StoryBlog.Web.Microservices.Posts.Shared.Messages;

public sealed record NewPostPublishedMessage(Guid PostKey, string Slug) : IHubMessage;