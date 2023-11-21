namespace StoryBlog.Web.Microservices.Identity.Application.Configuration;

public sealed class TokenGenerationOptions
{
    /// <summary>
    /// Gets or sets flag to include all users claims into ID token
    /// </summary>
    public bool IncludeAllIdentityClaims { get; set; } = true;
}