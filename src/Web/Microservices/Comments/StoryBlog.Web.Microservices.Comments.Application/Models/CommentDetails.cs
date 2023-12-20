using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Comments.Application.Models;

public class CommentDetails
{
    public Guid Key
    {
        get;
        set;
    }

    public Guid? ParentKey
    {
        get;
        set;
    }

    public Guid PostKey
    {
        get;
        set;
    }

    public string Text
    {
        get;
        set;
    }

    public string? Author
    {
        get;
        set;
    }

    public PublicationStatus PublicationStatus
    {
        get;
        set;
    }

    public IReadOnlyList<Comment> Comments
    {
        get;
        set;
    }

    public VisibilityStatus VisibilityStatus
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