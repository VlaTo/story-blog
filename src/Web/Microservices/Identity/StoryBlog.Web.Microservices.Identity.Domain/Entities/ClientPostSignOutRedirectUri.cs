using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

public sealed class ClientPostSignOutRedirectUri : IEntity, IHasId<int>
{
    public int Id
    {
        get;
        set;
    }

    public string PostLogoutRedirectUri
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