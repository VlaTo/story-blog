using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Microservices.Identity.Application.Configuration;

public class StoryBlogIdentityServerOptions
{
    public const string SectionName = "IdentityServerOptions";
}

[DataContract, Serializable]
public class StoryBlogIdentityServerOptions<TIdentityOptions> : StoryBlogIdentityServerOptions
    where TIdentityOptions : IIdentityOptions
{
    public TIdentityOptions Identity
    {
        get;
        set;
    }

    [Required]
    public string Secret
    {
        get;
        set;
    }

    [DefaultValue(true)]
    public bool AllowRememberMe
    {
        get;
        set;
    }

    [DefaultValue(true)]
    public bool AllowUserLockOut
    {
        get;
        set;
    }

    public TimeSpan RefreshTokenDuration
    {
        get;
        set;
    }

    public TimeSpan SecurityTokenDuration
    {
        get;
        set;
    }
}