using StoryBlog.Web.Microservices.Posts.Domain.Entities;

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

    public PostStatus Status
    {
        get;
        set;
    }

    public DateTime CreatedAt
    {
        get;
        set;
    }
}