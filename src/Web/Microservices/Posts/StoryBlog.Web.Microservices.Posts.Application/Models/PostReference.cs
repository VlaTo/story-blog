using StoryBlog.Web.Microservices.Posts.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Application.Models;

public sealed class PostReference : PostDetails
{
    public Guid Key
    {
        get;
        set;
    }

    public PostStatus Status
    {
        get;
        set;
    }
}