using MediatR;

namespace StoryBlog.Web.Microservices.Comments.Application.Handlers.PostPublished;

public sealed record PostPublishedCommand(
    Guid PostKey
) : INotification;