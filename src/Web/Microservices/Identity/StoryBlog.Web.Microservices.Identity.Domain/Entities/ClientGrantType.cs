using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

public sealed class ClientGrantType : IEntity, IHasId<int>
{
    public int Id
    {
        get;
        set;
    }

    public required string GrantType
    {
        get;
        set;
    }

    public int ClientId
    {
        get;
        set;
    }

    public required Client Client
    {
        get;
        set;
    }
}