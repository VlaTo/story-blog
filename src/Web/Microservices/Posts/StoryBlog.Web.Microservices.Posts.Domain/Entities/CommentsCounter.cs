using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Domain.Entities;

public sealed class CommentsCounter : Entity
{
    public long PostId
    {
        get;
        set;
    }

    public Post Post
    {
        get;
        set;
    }

    public long Counter
    {
        get;
        set;
    }
}