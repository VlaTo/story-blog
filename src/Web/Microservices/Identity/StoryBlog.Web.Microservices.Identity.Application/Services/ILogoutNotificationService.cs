using StoryBlog.Web.Microservices.Identity.Application.Contexts;

namespace StoryBlog.Web.Microservices.Identity.Application.Services;

/// <summary>
/// Provides features for OIDC signout notifications.
/// </summary>
public interface ILogoutNotificationService
{
    /// <summary>
    /// Builds the URLs needed for front-channel logout notification.
    /// </summary>
    /// <param name="context">The context for the logout notification.</param>
    Task<IEnumerable<string>> GetFrontChannelLogoutNotificationsUrlsAsync(LogoutNotificationContext context);

    /// <summary>
    /// Builds the http back-channel logout request data for the collection of clients.
    /// </summary>
    /// <param name="context">The context for the logout notification.</param>
    Task<IEnumerable<Models.Requests.BackChannelLogoutRequest>> GetBackChannelLogoutNotificationsAsync(LogoutNotificationContext context);
}