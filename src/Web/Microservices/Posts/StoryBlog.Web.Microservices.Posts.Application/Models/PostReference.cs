using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Application.Models;

public sealed class PostReference : PostDetails
{
    public Guid Key
    {
        get;
        set;
    }

    public PublicationStatus PublicationStatus
    {
        get;
        set;
    }
}