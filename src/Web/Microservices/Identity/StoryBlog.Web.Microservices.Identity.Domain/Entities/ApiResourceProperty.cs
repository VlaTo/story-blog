namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

public sealed class ApiResourceProperty : Property
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