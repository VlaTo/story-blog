﻿using Microsoft.AspNetCore.Http;
using StoryBlog.Web.Common.Identity.Permission;

namespace StoryBlog.Web.Microservices.Identity.Application.Configuration;

/// <summary>
/// Configures the login and logout views and behavior.
/// </summary>
public class AuthenticationOptions
{
    /// <summary>
    /// Sets the cookie authentication scheme configured by the host used for interactive users. If not set, the scheme will inferred from the host's default authentication scheme.
    /// This setting is typically used when AddPolicyScheme is used in the host as the default scheme.
    /// </summary>
    public string? CookieAuthenticationScheme
    {
        get;
        set;
    }

    /// <summary>
    /// Sets the cookie lifetime (only effective if the IdentityServer-provided cookie handler is used)
    /// </summary>
    public TimeSpan CookieLifetime
    {
        get;
        set;
    }

    /// <summary>
    /// Specified if the cookie should be sliding or not (only effective if the built-in cookie middleware is used)
    /// </summary>
    public bool CookieSlidingExpiration
    {
        get;
        set;
    }

    /// <summary>
    /// Specifies the SameSite mode for the internal authentication and temp cookie
    /// </summary>
    public SameSiteMode CookieSameSiteMode
    {
        get;
        set;
    }

    /// <summary>
    /// Indicates if user must be authenticated to accept parameters to end session endpoint. Defaults to false.
    /// </summary>
    /// <value>
    /// <c>true</c> if required; otherwise, <c>false</c>.
    /// </value>
    public bool RequireAuthenticatedUserForSignOutMessage
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the name of the cookie used for the check session endpoint.
    /// </summary>
    public string? CheckSessionCookieName
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the domain of the cookie used for the check session endpoint. Defaults to null.
    /// </summary>
    public string? CheckSessionCookieDomain
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the SameSite mode of the cookie used for the check session endpoint. Defaults to SameSiteMode.None.
    /// </summary>
    public SameSiteMode CheckSessionCookieSameSiteMode
    {
        get;
        set;
    }

    /// <summary>
    /// If set, will require frame-src CSP headers being emitting on the end session callback endpoint which renders iframes to clients for front-channel sign out notification.
    /// </summary>
    public bool RequireCspFrameSrcForSignOut
    {
        get;
        set;
    }

    /// <summary>
    /// The claim type used for the user's display name.
    /// This is used when storing user sessions server side.
    /// </summary>
    public string? UserDisplayNameClaimType
    {
        get;
        set;
    }

    public AuthenticationOptions()
    {
        CookieLifetime = IdentityServerConstants.DefaultCookieTimeSpan;
        CookieSlidingExpiration = false;
        CookieSameSiteMode = SameSiteMode.None;
        RequireAuthenticatedUserForSignOutMessage = false;
        CheckSessionCookieName = IdentityServerConstants.DefaultCheckSessionCookieName;
        CheckSessionCookieSameSiteMode = SameSiteMode.None;
        RequireCspFrameSrcForSignOut = true;
    }
}