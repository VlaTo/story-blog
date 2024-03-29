using MediatR;

namespace StoryBlog.Web.Microservices.Communication.Application.Handlers.NewPostCreated;

public sealed class NewPostCreatedCommand : INotification
{
    public required Guid PostKey
    {
        get;
        set;
    }

    public required DateTimeOffset Created
    {
        get;
        set;
    }

    public required string Slug
    {
        get;
        set;
    }

    public required string AuthorId
    {
        get;
        set;
    }
}