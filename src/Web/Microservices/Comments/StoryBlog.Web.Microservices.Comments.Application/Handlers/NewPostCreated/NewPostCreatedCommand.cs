using MediatR;

namespace StoryBlog.Web.Microservices.Comments.Application.Handlers.NewPostCreated;

public sealed record NewPostCreatedCommand(
    Guid PostKey
) : INotification;