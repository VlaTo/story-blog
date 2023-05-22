namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

public sealed class IdentityResourceClaim : UserClaim
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