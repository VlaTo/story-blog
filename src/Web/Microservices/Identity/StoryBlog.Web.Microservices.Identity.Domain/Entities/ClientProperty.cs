namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

public sealed class ClientProperty : Property
{
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