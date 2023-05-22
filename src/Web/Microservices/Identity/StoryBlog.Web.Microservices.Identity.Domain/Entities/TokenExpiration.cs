namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

/// <summary>
/// Token expiration types.
/// </summary>
public enum TokenExpiration
{
    /// <summary>
    /// Sliding token expiration
    /// </summary>
    Sliding = 0,

    /// <summary>
    /// Absolute token expiration
    /// </summary>
    Absolute = 1
}