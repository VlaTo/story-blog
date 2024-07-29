using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

public abstract class UserClaim : IEntity, IHasId<int>
{
    public int Id
    {
        get;
        set;
    }

    public required string Type
    {
        get;
        set;
    }
}