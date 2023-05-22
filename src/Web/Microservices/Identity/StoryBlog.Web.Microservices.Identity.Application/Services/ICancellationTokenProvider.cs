namespace StoryBlog.Web.Microservices.Identity.Application.Services;

/// <summary>
/// Service to provide CancellationToken for async operations.
/// </summary>
public interface ICancellationTokenProvider
{
    /// <summary>
    /// Returns the current CancellationToken, or null if none present.
    /// </summary>
    CancellationToken CancellationToken
    {
        get;
    }
}