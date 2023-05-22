namespace StoryBlog.Web.Microservices.Identity.Application.Services;

/// <summary>
/// Implementation of ICancellationTokenProvider that returns CancellationToken.None
/// </summary>
public sealed class NoneCancellationTokenProvider : ICancellationTokenProvider
{
    /// <inheritdoc/>
    public CancellationToken CancellationToken => CancellationToken.None;
}