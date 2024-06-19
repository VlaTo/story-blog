using MediatR;

namespace StoryBlog.Web.Microservices.Comments.Application.Handlers.PostDeleted;

public sealed record PostDeletedCommand(
    Guid PostKey
) : INotification;