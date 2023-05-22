using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

public class ClientCorsOrigin : IEntity, IHasId<int>
{
    public int Id
    {
        get;
        set;
    }

    public string Origin
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