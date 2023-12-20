using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Domain.Entities;

public class Post : Entity, IHasId<long>
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

    public string Title
    {
        get;
        set;
    }

    public Slug Slug
    {
        get;
        set;
    }

    public Content Content
    {
        get;
        set;
    }

    public CommentsCounter CommentsCounter
    {
        get;
        set;
    }

    public PublicationStatus PublicationStatus
    {
        get;
        set;
    }

    public VisibilityStatus VisibilityStatus
    {
        get;
        set;
    }

    public string AuthorId
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