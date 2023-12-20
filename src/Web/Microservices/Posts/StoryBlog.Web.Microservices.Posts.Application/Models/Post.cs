using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Application.Models;

public sealed class Post : PostDetails
{
    public Guid Key
    {
        get;
        set;
    }

    public string Text
    {
        get;
        set;
    }

    public PublicationStatus PublicationStatus
    {
        get;
        set;
    }

    public long CommentsCount
    {
        get;
        set;
    }

    public DateTimeOffset CreatedAt
    {
        get;
        set;
    }
}