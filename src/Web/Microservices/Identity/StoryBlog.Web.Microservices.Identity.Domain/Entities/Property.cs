using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

public abstract class Property : IEntity, IHasId<int>
{
    public int Id
    {
        get;
        set;
    }

    public string Key
    {
        get;
        set;
    }

    public string Value
    {
        get;
        set;
    }
}