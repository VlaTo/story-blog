using StoryBlog.Web.MessageHub.Messages;

namespace StoryBlog.Web.Microservices.Communication.Shared.Messages;

public sealed record NewPostPublishedMessage(Guid PostKey, string Slug) : IHubMessage;