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

    public bool IsPublic
    {
        get;
        set;
    }

    public PostStatus Status
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

    public DateTimeOffset? DeletedAt
    {
        get;
        set;
    }
}