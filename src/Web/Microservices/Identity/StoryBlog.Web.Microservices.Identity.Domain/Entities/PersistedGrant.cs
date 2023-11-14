using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

public class PersistedGrant : IEntity, IHasId<long>
{
    public long Id
    {
        get;
        set;
    }

    public string Key
    {
        get;
        set;
    }

    public string Type
    {
        get;
        set;
    }

    public string SubjectId
    {
        get;
        set;
    }

    public string SessionId
    {
        get;
        set;
    }

    public string ClientId
    {
        get;
        set;
    }

    public string? Description
    {
        get;
        set;
    }

    public DateTimeOffset CreationTime
    {
        get;
        set;
    }

    public DateTimeOffset? Expiration
    {
        get;
        set;
    }

    public DateTimeOffset? ConsumedTime
    {
        get;
        set;
    }

    public string? Data
    {
        get;
        set;
    }
}