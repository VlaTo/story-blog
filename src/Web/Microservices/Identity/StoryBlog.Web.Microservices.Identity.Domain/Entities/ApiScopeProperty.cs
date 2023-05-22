namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

public sealed class ApiScopeProperty : Property
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