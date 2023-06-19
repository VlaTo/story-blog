using StoryBlog.Web.Microservices.Identity.Application.Models.Requests;

namespace StoryBlog.Web.Microservices.Identity.Application.Services;

/// <summary>
/// Interface for sending a user a login request from a backchannel authentication request.
/// </summary>
public interface IBackchannelAuthenticationUserNotificationService
{
    /// <summary>
    /// Sends a notification for the user to login.
    /// </summary>
    Task SendLoginRequestAsync(BackchannelUserLoginRequest request);
}