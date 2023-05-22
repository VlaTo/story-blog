namespace StoryBlog.Web.Microservices.Identity.Application.Configuration;

internal sealed class ClientDefinition : ServiceDefinition
{
    public string? RedirectUri
    {
        get;
        set;
    }

    public string? LogoutUri
    {
        get;
        set;
    }

    public string ClientSecret
    {
        get;
        set;
    }
}