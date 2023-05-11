using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Domain.Entities;

public sealed class Content : Entity
{
    public string Text
    {
        get;
        set;
    }

    public string Brief
    {
        get;
        set;
    }

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

}