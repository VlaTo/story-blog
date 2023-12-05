using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Domain.Entities;

public enum PostProcessStatus
{
    Failed = -1,
    Pending,
    Processing,
    Success,
    Skipped
}

public sealed class PostProcessTask : Entity, IHasId<int>
{
    public int Id
    {
        get;
        set;
    }

    public Guid Key
    {
        get;
        set;
    }

    public Guid PostKey
    {
        get;
        set;
    }

    public PostProcessStatus Status
    {
        get;
        set;
    }

    public DateTimeOffset Created
    {
        get;
        set;
    }

    public DateTimeOffset? Completed
    {
        get;
        set;
    }
}