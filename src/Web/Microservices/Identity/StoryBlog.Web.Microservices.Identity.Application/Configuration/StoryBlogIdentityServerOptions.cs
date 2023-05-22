using System.Runtime.Serialization;

namespace StoryBlog.Web.Microservices.Identity.Application.Configuration;

internal class StoryBlogIdentityServerOptions
{
    public const string SectionName = "IdentityServerOptions";
}

[DataContract, Serializable]
internal class StoryBlogIdentityServerOptions<TIdentityOptions> : StoryBlogIdentityServerOptions
    where TIdentityOptions : IIdentityOptions
{
    public TIdentityOptions IdentityOptions
    {
        get;
        set;
    }
}