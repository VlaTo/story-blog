using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

public class IdentityResource : IEntity, IHasId<int>
{
    public int Id
    {
        get;
        set;
    }

    public bool Enabled
    {
        get;
        set;
    }

    public string Name
    {
        get;
        set;
    }

    public string DisplayName
    {
        get;
        set;
    }

    public string Description
    {
        get;
        set;
    }

    public bool Required
    {
        get;
        set;
    }

    public bool Emphasize
    {
        get;
        set;
    }

    public bool ShowInDiscoveryDocument
    {
        get;
        set;
    }

    public List<IdentityResourceClaim> UserClaims
    {
        get;
        set;
    }

    public List<IdentityResourceProperty> Properties
    {
        get;
        set;
    }

    public DateTimeOffset Created
    {
        get;
        set;
    }

    public DateTimeOffset? Updated
    {
        get;
        set;
    }

    public bool NonEditable
    {
        get;
        set;
    }

    public IdentityResource()
    {
        Enabled = true;
        ShowInDiscoveryDocument = true;
        UserClaims = new List<IdentityResourceClaim>();
        Properties = new List<IdentityResourceProperty>();
        //Created = DateTime.UtcNow;
    }
}