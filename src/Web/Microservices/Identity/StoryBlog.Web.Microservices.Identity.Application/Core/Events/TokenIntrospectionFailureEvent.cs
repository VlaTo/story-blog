﻿namespace StoryBlog.Web.Microservices.Identity.Application.Core.Events;

/// <summary>
/// Event for failed token introspection
/// </summary>
/// <seealso cref="Event" />
public sealed class TokenIntrospectionFailureEvent : Event
{
    /// <summary>
    /// Gets or sets the name of the API.
    /// </summary>
    /// <value>
    /// The name of the API.
    /// </value>
    public string ApiName
    {
        get; 
        set;
    }

    /// <summary>
    /// Gets or sets the token.
    /// </summary>
    /// <value>
    /// The token.
    /// </value>
    public string? Token
    {
        get; 
        set;
    }

    /// <summary>
    /// Gets or sets the API scopes.
    /// </summary>
    /// <value>
    /// The API scopes.
    /// </value>
    public IEnumerable<string>? ApiScopes
    {
        get; 
        set;
    }

    /// <summary>
    /// Gets or sets the token scopes.
    /// </summary>
    /// <value>
    /// The token scopes.
    /// </value>
    public IEnumerable<string>? TokenScopes
    {
        get; 
        set;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenIntrospectionSuccessEvent" /> class.
    /// </summary>
    /// <param name="apiName">Name of the API.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="token">The token.</param>
    /// <param name="apiScopes">The API scopes.</param>
    /// <param name="tokenScopes">The token scopes.</param>
    public TokenIntrospectionFailureEvent(
        string apiName,
        string errorMessage,
        string? token = null,
        IEnumerable<string>? apiScopes = null,
        IEnumerable<string>? tokenScopes = null)
        : base(EventCategories.Token,
            "Token Introspection Failure",
            EventTypes.Failure,
            EventIds.TokenIntrospectionFailure,
            errorMessage)
    {
        ApiName = apiName;

        if (false == String.IsNullOrWhiteSpace(token))
        {
            Token = Obfuscate(token);
        }

        if (null != apiScopes)
        {
            ApiScopes = apiScopes;
        }

        if (null != tokenScopes)
        {
            TokenScopes = tokenScopes;
        }
    }
}