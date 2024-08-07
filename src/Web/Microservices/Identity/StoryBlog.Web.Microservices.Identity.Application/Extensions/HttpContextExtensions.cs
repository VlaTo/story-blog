﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using StoryBlog.Web.Common.Identity.Permission;
using StoryBlog.Web.Microservices.Identity.Application.Configuration;
using StoryBlog.Web.Microservices.Identity.Application.Contexts;
using StoryBlog.Web.Microservices.Identity.Application.Models.Messages;
using StoryBlog.Web.Microservices.Identity.Application.Services;
using StoryBlog.Web.Microservices.Identity.Application.Stores;

namespace StoryBlog.Web.Microservices.Identity.Application.Extensions;

internal static class HttpContextExtensions
{
    internal static async Task<string> GetCookieAuthenticationSchemeAsync(this HttpContext context)
    {
        var options = context.RequestServices.GetRequiredService<IdentityServerOptions>();

        if (null != options.Authentication.CookieAuthenticationScheme)
        {
            return options.Authentication.CookieAuthenticationScheme;
        }

        var schemes = context.RequestServices.GetRequiredService<IAuthenticationSchemeProvider>();
        var scheme = await schemes.GetDefaultAuthenticateSchemeAsync();

        if (null == scheme)
        {
            throw new InvalidOperationException("No DefaultAuthenticateScheme found or no CookieAuthenticationScheme configured on IdentityServerOptions.");
        }

        return scheme.Name;
    }
    
    internal static async Task<string?> GetIdentityServerSignOutFrameCallbackUrlAsync(
        this HttpContext? context,
        LogoutMessage? logoutMessage = null)
    {
        if (null == context)
        {
            return null;
        }

        var userSession = context.RequestServices.GetRequiredService<IUserSession>();
        var user = await userSession.GetUserAsync();
        var currentSubId = user?.GetSubjectId();

        LogoutNotificationContext? endSessionMsg = null;

        // if we have a logout message, then that take precedence over the current user
        if (true == logoutMessage?.ClientIds?.Any())
        {
            var clientIds = logoutMessage.ClientIds;

            // check if current user is same, since we might have new clients (albeit unlikely)
            if (currentSubId == logoutMessage.SubjectId)
            {
                var clientList = await userSession.GetClientListAsync();
                clientIds = clientIds.Union(clientList).Distinct();
            }

            endSessionMsg = new LogoutNotificationContext
            {
                SubjectId = logoutMessage.SubjectId,
                SessionId = logoutMessage.SessionId,
                ClientIds = clientIds
            };
        }
        else if (null != currentSubId)
        {
            // see if current user has any clients they need to signout of 
            var clientIds = await userSession.GetClientListAsync();

            if (clientIds.Any())
            {
                endSessionMsg = new LogoutNotificationContext
                {
                    SubjectId = currentSubId,
                    SessionId = await userSession.GetSessionIdAsync(),
                    ClientIds = clientIds
                };
            }
        }

        if (null != endSessionMsg)
        {
            var clock = context.RequestServices.GetRequiredService<ISystemClock>();
            var msg = new Message<LogoutNotificationContext>(endSessionMsg, clock.UtcNow.UtcDateTime);

            var endSessionMessageStore = context.RequestServices.GetRequiredService<IMessageStore<LogoutNotificationContext>>();
            var id = await endSessionMessageStore.WriteAsync(msg);

            var urls = context.RequestServices.GetRequiredService<IServerUrls>();
            var signoutIframeUrl = urls.BaseUrl.EnsureTrailingSlash() + Constants.ProtocolRoutePaths.EndSessionCallback;

            signoutIframeUrl = signoutIframeUrl.AddQueryString(Constants.UIConstants.DefaultRoutePathParams.EndSessionCallback, id);

            return signoutIframeUrl;
        }

        // no sessions, so nothing to cleanup
        return null;
    }

    internal static void SetSignOutCalled(this HttpContext context)
    {
        context.Items[Constants.EnvironmentKeys.SignOutCalled] = "true";
    }

    internal static bool GetSignOutCalled(this HttpContext context)
    {
        return context.Items.ContainsKey(Constants.EnvironmentKeys.SignOutCalled);
    }
}