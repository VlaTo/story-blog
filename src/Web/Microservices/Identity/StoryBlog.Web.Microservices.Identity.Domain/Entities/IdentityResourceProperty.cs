namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

public sealed class IdentityResourceProperty : Property
{
    public int IdentityResourceId
    {
        get;
        set;
    }

    public IdentityResource IdentityResource
    {
        get;
        set;
    }
}