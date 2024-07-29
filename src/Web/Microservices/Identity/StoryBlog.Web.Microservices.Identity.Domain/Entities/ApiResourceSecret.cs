namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

public sealed class ApiResourceSecret : Secret
{
    public int ApiResourceId
    {
        get;
        set;
    }

    public required ApiResource ApiResource
    {
        get;
        set;
    }
}