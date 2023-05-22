namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

public class ClientSecret : Secret
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