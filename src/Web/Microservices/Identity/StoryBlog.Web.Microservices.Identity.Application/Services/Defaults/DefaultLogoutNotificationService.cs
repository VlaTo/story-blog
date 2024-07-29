﻿using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Identity.Permission;
using StoryBlog.Web.Microservices.Identity.Application.Contexts;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Application.Models.Requests;
using StoryBlog.Web.Microservices.Identity.Application.Stores;
using OidcConstants = IdentityModel.OidcConstants;

namespace StoryBlog.Web.Microservices.Identity.Application.Services.Defaults;

/// <summary>
/// Default implementation of logout notification service.
/// </summary>
public class DefaultLogoutNotificationService : ILogoutNotificationService
{
    private readonly IClientStore clientStore;
    private readonly IIssuerNameService issuerNameService;
    private readonly ILogger<DefaultLogoutNotificationService> logger;

    /// <summary>
    /// Ctor.
    /// </summary>
    public DefaultLogoutNotificationService(
        IClientStore clientStore,
        IIssuerNameService issuerNameService,
        ILogger<DefaultLogoutNotificationService> logger)
    {
        this.clientStore = clientStore;
        this.issuerNameService = issuerNameService;
        this.logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<string>> GetFrontChannelLogoutNotificationsUrlsAsync(LogoutNotificationContext context)
    {
        using var activity = Tracing.ServiceActivitySource.StartActivity("LogoutNotificationService.GetFrontChannelLogoutNotificationsUrls");

        var frontChannelUrls = new List<string>();

        foreach (var clientId in context.ClientIds)
        {
            var client = await clientStore.FindEnabledClientByIdAsync(clientId);

            if (null != client)
            {
                if (client.FrontChannelLogoutUri.IsPresent())
                {
                    var url = client.FrontChannelLogoutUri;

                    // add session id if required
                    if (IdentityServerConstants.ProtocolTypes.OpenIdConnect == client.ProtocolType)
                    {
                        if (client.FrontChannelLogoutSessionRequired)
                        {
                            url = url
                                .AddQueryString(OidcConstants.EndSessionRequest.Sid, context.SessionId)
                                .AddQueryString(OidcConstants.EndSessionRequest.Issuer, await issuerNameService.GetCurrentAsync());
                        }
                    }
                    else if (client.ProtocolType == IdentityServerConstants.ProtocolTypes.WsFederation)
                    {
                        url = url.AddQueryString(Constants.WsFedSignOut.LogoutUriParameterName, Constants.WsFedSignOut.LogoutUriParameterValue);
                    }

                    frontChannelUrls.Add(url);
                }
            }
        }

        if (frontChannelUrls.Any())
        {
            var msg = frontChannelUrls.Aggregate((x, y) => x + ", " + y);
            logger.LogDebug("Client front-channel logout URLs: {0}", msg);
        }
        else
        {
            logger.LogDebug("No client front-channel logout URLs");
        }

        return frontChannelUrls;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<BackChannelLogoutRequest>> GetBackChannelLogoutNotificationsAsync(LogoutNotificationContext context)
    {
        using var activity = Tracing.ServiceActivitySource.StartActivity("LogoutNotificationService.GetBackChannelLogoutNotifications");

        var backChannelLogouts = new List<BackChannelLogoutRequest>();

        foreach (var clientId in context.ClientIds)
        {
            var client = await clientStore.FindEnabledClientByIdAsync(clientId);

            if (null != client)
            {
                if (client.BackChannelLogoutUri.IsPresent())
                {
                    var back = new BackChannelLogoutRequest
                    {
                        ClientId = clientId,
                        LogoutUri = client.BackChannelLogoutUri,
                        SubjectId = context.SubjectId,
                        SessionId = context.SessionId,
                        SessionIdRequired = client.BackChannelLogoutSessionRequired,
                        Issuer = context.Issuer,
                    };

                    backChannelLogouts.Add(back);
                }
            }
        }

        if (backChannelLogouts.Any())
        {
            var msg = backChannelLogouts.Select(x => x.LogoutUri).Aggregate((x, y) => x + ", " + y);
            logger.LogDebug("Client back-channel logout URLs: {0}", msg);
        }
        else
        {
            logger.LogDebug("No client back-channel logout URLs");
        }

        return backChannelLogouts;
    }
}