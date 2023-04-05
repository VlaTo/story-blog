using System.Runtime.Serialization;

namespace StoryBlog.Web.Identity.Configuration;

[DataContract]
[Serializable]
public sealed class StoryBlogAuthenticationOptions
{
    public const string SectionName = "StoryBlogAuthentication";

    [DataMember]
    public JwtBearerAuthenticationOptions JwtBearer
    {
        get;
        set;
    }
}