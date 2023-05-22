namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

public sealed class ApiResourceClaim : UserClaim
{
    public int ApiResourceId
    {
        get;
        set;
    }

    public ApiResource ApiResource
    {
        get;
        set;
    }
}