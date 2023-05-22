namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

public sealed class ApiScopeClaim : UserClaim
{
    public int ScopeId
    {
        get;
        set;
    }

    public ApiScope Scope
    {
        get;
        set;
    }
}