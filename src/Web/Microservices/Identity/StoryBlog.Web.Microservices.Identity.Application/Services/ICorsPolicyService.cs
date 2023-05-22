namespace StoryBlog.Web.Microservices.Identity.Application.Services;

/// <summary>
/// Service that determines if CORS is allowed.
/// </summary>
public interface ICorsPolicyService
{
    /// <summary>
    /// Determines whether origin is allowed.
    /// </summary>
    /// <param name="origin">The origin.</param>
    /// <returns></returns>
    Task<bool> IsOriginAllowedAsync(string origin);
}