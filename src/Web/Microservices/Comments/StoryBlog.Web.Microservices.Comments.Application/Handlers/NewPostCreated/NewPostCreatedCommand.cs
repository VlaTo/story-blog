using MediatR;

namespace StoryBlog.Web.Microservices.Comments.Application.Handlers.NewPostCreated;

public sealed class NewPostCreatedCommand : INotification
{
    public required Guid PostKey
    {
        get;
        set;
    }
}