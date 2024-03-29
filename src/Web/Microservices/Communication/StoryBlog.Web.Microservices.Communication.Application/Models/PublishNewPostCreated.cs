namespace StoryBlog.Web.Microservices.Communication.Application.Models;

public sealed class PublishNewPostCreated
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