using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

public sealed class IdentityProvider : IEntity, IHasId<int>
{
    /// <summary>
    /// Primary key used for EF
    /// </summary>
    public int Id
    {
        get;
        set;
    }

    /// <summary>
    /// Scheme name for the provider.
    /// </summary>
    public string Scheme
    {
        get;
        set;
    }

    /// <summary>
    /// Display name for the provider.
    /// </summary>
    public string DisplayName
    {
        get;
        set;
    }

    /// <summary>
    /// Flag that indicates if the provider should be used.
    /// </summary>
    public bool Enabled
    {
        get;
        set;
    }

    /// <summary>
    /// Protocol type of the provider.
    /// </summary>
    public string Type
    {
        get;
        set;
    }

    /// <summary>
    /// Serialized value for the identity provider properties dictionary.
    /// </summary>
    public string Properties
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

    public IdentityProvider()
    {
        Enabled = true;
        //Created = DateTime.UtcNow;
    }
}