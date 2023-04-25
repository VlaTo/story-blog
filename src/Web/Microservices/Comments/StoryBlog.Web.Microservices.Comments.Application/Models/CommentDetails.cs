using StoryBlog.Web.Microservices.Comments.Domain.Entities;

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

    public CommentStatus Status
    {
        get;
        set;
    }

    public IReadOnlyList<Comment> Comments
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