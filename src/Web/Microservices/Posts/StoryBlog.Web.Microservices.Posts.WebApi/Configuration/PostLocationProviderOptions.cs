namespace StoryBlog.Web.Microservices.Posts.WebApi.Configuration;

public sealed class PostLocationProviderOptions
{
    internal const string SectionName = "PostLocationProvider";

    public bool UseUrlHelper { get; set; }

    public string? ExternalUrlTemplate { get; set; }
}