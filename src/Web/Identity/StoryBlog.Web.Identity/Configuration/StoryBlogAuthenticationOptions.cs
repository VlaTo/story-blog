namespace StoryBlog.Web.Identity.Configuration;

[Serializable]
public sealed class StoryBlogAuthenticationOptions
{
    public const string SectionName = "StoryBlogAuthentication";

    public required JwtBearerAuthenticationOptions JwtBearer
    {
        get;
        set;
    }
}