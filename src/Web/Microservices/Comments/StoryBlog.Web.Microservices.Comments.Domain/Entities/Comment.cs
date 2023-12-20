using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Comments.Domain.Entities;

public sealed class Comment : Entity, IHasId<long>
{
    public long Id
    {
        get;
        set;
    }

    public Guid Key
    {
        get;
        set;
    }

    public long? ParentId
    {
        get;
        set;
    }

    public Guid PostKey
    {
        get;
        set;
    }

    public string? AuthorId
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

    public Comment? Parent
    {
        get;
        set;
    }

    public VisibilityStatus VisibilityStatus
    {
        get;
        set;
    }

    public List<Comment> Comments
    {
        get;
        set;
    }

    public DateTimeOffset CreateAt
    {
        get;
        set;
    }

    public DateTimeOffset? ModifiedAt
    {
        get;
        set;
    }
}