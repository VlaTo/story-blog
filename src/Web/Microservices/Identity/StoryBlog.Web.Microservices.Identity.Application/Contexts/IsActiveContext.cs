﻿using StoryBlog.Web.Microservices.Identity.Application.Storage;
using System.Security.Claims;

namespace StoryBlog.Web.Microservices.Identity.Application.Contexts;

/// <summary>
/// Context describing the is-active check
/// </summary>
public sealed class IsActiveContext
{
    /// <summary>
    /// Gets or sets the subject.
    /// </summary>
    /// <value>
    /// The subject.
    /// </value>
    public ClaimsPrincipal Subject
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the client.
    /// </summary>
    /// <value>
    /// The client.
    /// </value>
    public Client Client
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the caller.
    /// </summary>
    /// <value>
    /// The caller.
    /// </value>
    public string Caller
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the subject is active and can receive tokens.
    /// </summary>
    /// <value>
    ///   <c>true</c> if the subject is active; otherwise, <c>false</c>.
    /// </value>
    public bool IsActive
    {
        get;
        set;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IsActiveContext"/> class.
    /// </summary>
    public IsActiveContext(ClaimsPrincipal subject, Client client, string caller)
    {
        Subject = subject;
        Client = client;
        Caller = caller;

        IsActive = true;
    }
}