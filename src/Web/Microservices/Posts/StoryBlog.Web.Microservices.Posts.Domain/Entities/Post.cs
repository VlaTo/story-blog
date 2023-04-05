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

    public DateTime CreateAt
    {
        get;
        set;
    }

    public DateTime? ModifiedAt
    {
        get;
        set;
    }

    public DateTime? DeletedAt
    {
        get;
        set;
    }
}