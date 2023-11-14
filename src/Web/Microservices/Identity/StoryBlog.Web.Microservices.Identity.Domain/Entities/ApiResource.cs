using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

public class ApiResource : IEntity, IHasId<int>
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

    public string AllowedAccessTokenSigningAlgorithms
    {
        get;
        set;
    }

    public bool ShowInDiscoveryDocument
    {
        get;
        set;
    }

    public bool RequireResourceIndicator
    {
        get;
        set;
    }

    public List<ApiResourceSecret> Secrets
    {
        get;
        set;
    }

    public List<ApiResourceScope> Scopes
    {
        get;
        set;
    }

    public List<ApiResourceClaim> UserClaims
    {
        get;
        set;
    }

    public List<ApiResourceProperty> Properties
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

    public DateTimeOffset? LastAccessed
    {
        get;
        set;
    }

    public bool NonEditable
    {
        get;
        set;
    }

    public ApiResource()
    {
        Enabled = true;
        ShowInDiscoveryDocument = true;
    }
}