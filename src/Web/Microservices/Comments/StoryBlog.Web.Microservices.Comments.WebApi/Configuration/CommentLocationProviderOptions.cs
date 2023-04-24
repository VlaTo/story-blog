namespace StoryBlog.Web.Microservices.Comments.WebApi.Configuration;

public class CommentLocationProviderOptions
{
    internal const string SectionName = "LocationProvider";

    public bool UseUrlHelper { get; set; }

    public string? ExternalUrlTemplate { get; set; }
}