using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

public sealed class ClientClaim : IEntity, IHasId<int>
{
    public int Id
    {
        get;
        set;
    }

    public string Type
    {
        get;
        set;
    }

    public string Value
    {
        get;
        set;
    }

    public int ClientId
    {
        get;
        set;
    }

    public Client Client
    {
        get;
        set;
    }
}