using StoryBlog.Web.Microservices.Identity.Application.Validation.Requests;

namespace StoryBlog.Web.Microservices.Identity.Application.Services;

/// <summary>
/// The backchannel authentication throttling service.
/// </summary>
public interface IBackChannelAuthenticationThrottlingService
{
    /// <summary>
    /// Decides if the requesting client and request needs to slow down.
    /// </summary>
    Task<bool> ShouldSlowDown(string requestId, BackChannelAuthenticationRequest details);
}