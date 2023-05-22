﻿using System.Security.Claims;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation.Requests;

/// <summary>
/// Models a backchannel authentication request.
/// </summary>
public class BackChannelAuthenticationRequest
{
    /// <summary>
    /// The identifier for this request in the store.
    /// </summary>
    public string InternalId
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the creation time.
    /// </summary>
    public DateTime CreationTime
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the life time in seconds.
    /// </summary>
    public TimeSpan Lifetime
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the ID of the client.
    /// </summary>
    public string ClientId
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the subject.
    /// </summary>
    public ClaimsPrincipal Subject
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the requested scopes.
    /// </summary>
    public IEnumerable<string> RequestedScopes
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the requested resource indicators.
    /// </summary>
    public IEnumerable<string> RequestedResourceIndicators
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the authentication context reference classes.
    /// </summary>
    public ICollection<string> AuthenticationContextReferenceClasses
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the tenant.
    /// </summary>
    public string Tenant
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the idp.
    /// </summary>
    public string IdP
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the binding message.
    /// </summary>
    public string BindingMessage
    {
        get;
        set;
    }


    /// <summary>
    /// Gets or sets a value indicating whether this instance has been completed.
    /// </summary>
    public bool IsComplete
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the authorized scopes.
    /// </summary>
    public IEnumerable<string> AuthorizedScopes
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the session identifier from which the user approved the request.
    /// </summary>
    public string SessionId
    {
        get;
        set;
    }

    /// <summary>
    /// Gets the description the user assigned to the client being authorized.
    /// </summary>
    public string Description
    {
        get;
        set;
    }
}