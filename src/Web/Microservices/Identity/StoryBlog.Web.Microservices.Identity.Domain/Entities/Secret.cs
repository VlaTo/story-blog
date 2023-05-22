using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

public abstract class Secret : IEntity, IHasId<int>
{
    public int Id
    {
        get;
        set;
    }

    public string Description
    {
        get;
        set;
    }

    public string Value
    {
        get;
        set;
    }

    public DateTime? Expiration
    {
        get;
        set;
    }

    public string Type
    {
        get;
        set;
    }

    public DateTime Created
    {
        get;
        set;
    }

    protected Secret()
    {
        Type = SecretTypes.SharedSecret;
        Created = DateTime.UtcNow;
    }
}