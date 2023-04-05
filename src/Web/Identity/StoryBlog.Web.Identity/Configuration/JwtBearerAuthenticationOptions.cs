using System.Runtime.Serialization;

namespace StoryBlog.Web.Identity.Configuration;

[DataContract]
[Serializable]
public sealed class JwtBearerAuthenticationOptions
{
    [DataMember]
    public bool RequireHttpsMetadata
    {
        get;
        set;
    }

    [DataMember]
    public string IssuerSigningKey
    {
        get;
        set;
    }

    [DataMember]
    public bool? ValidateIssuer
    {
        get;
        set;
    }

    [DataMember]
    public bool? ValidateAudience
    {
        get;
        set;
    }

    [DataMember]
    public bool? ValidateActor
    {
        get;
        set;
    }

    [DataMember]
    public bool? ValidateLifetime
    {
        get;
        set;
    }

    [DataMember]
    public string? Audience
    {
        get;
        set;
    }

    [DataMember]
    public string? Authority
    {
        get;
        set;
    }
}