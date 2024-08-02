namespace StoryBlog.Web.Identity.DependencyInjection.Configuration;

[Serializable]
public sealed class JwtBearerAuthenticationOptions
{
    public bool RequireHttpsMetadata
    {
        get;
        set;
    }

    public required string IssuerSigningKey
    {
        get;
        set;
    }

    public bool? ValidateIssuer
    {
        get;
        set;
    }

    public bool? ValidateAudience
    {
        get;
        set;
    }

    public bool? ValidateActor
    {
        get;
        set;
    }

    public bool? ValidateLifetime
    {
        get;
        set;
    }

    public string? Audience
    {
        get;
        set;
    }

    public string? Authority
    {
        get;
        set;
    }
}