﻿using StoryBlog.Web.Common.Identity.Permission;

namespace StoryBlog.Web.Microservices.Identity.Application.Core.Events;

/// <summary>
/// Event for failed user authentication
/// </summary>
/// <seealso cref="Event" />
public class UserSignInFailureEvent : Event
{
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    /// <value>
    /// The username.
    /// </value>
    public string Username
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the endpoint.
    /// </summary>
    /// <value>
    /// The endpoint.
    /// </value>
    public string Endpoint
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the client id.
    /// </summary>
    /// <value>
    /// The client id.
    /// </value>
    public string? ClientId
    {
        get;
        set;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Duende.IdentityServer.Events.UserSignInFailureEvent" /> class.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="error">The error.</param>
    /// <param name="interactive">Specifies if login was interactive</param>
    /// <param name="clientId">The client id</param>
    public UserSignInFailureEvent(string username, string error, bool interactive = true, string? clientId = null)
        : base(EventCategories.Authentication,
            "User Login Failure",
            EventTypes.Failure,
            EventIds.UserLoginFailure,
            error)
    {
        Username = username;
        ClientId = clientId;

        if (interactive)
        {
            Endpoint = "UI";
        }
        else
        {
            Endpoint = Constants.EndpointNames.Token;
        }
    }
}