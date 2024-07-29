using StoryBlog.Web.Common.Identity.Permission;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Requests;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Results;

namespace StoryBlog.Web.Microservices.Identity.Application.Core.Events;

/// <summary>
/// Event for failed backchannel authentication result
/// </summary>
/// <seealso cref="Event" />
public sealed class BackchannelAuthenticationFailureEvent : Event
{
    /// <summary>
    /// Gets or sets the client identifier.
    /// </summary>
    /// <value>
    /// The client identifier.
    /// </value>
    public string? ClientId
    {
        get; 
        set;
    }

    /// <summary>
    /// Gets or sets the name of the client.
    /// </summary>
    /// <value>
    /// The name of the client.
    /// </value>
    public string? ClientName
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
    /// Gets or sets the subject identifier.
    /// </summary>
    /// <value>
    /// The subject identifier.
    /// </value>
    public string? SubjectId
    {
        get; 
        set;
    }

    /// <summary>
    /// Gets or sets the scopes.
    /// </summary>
    /// <value>
    /// The scopes.
    /// </value>
    public string? Scopes
    {
        get; 
        set;
    }

    /// <summary>
    /// Gets or sets the error.
    /// </summary>
    /// <value>
    /// The error.
    /// </value>
    public string? Error
    {
        get; 
        set;
    }

    /// <summary>
    /// Gets or sets the error description.
    /// </summary>
    /// <value>
    /// The error description.
    /// </value>
    public string? ErrorDescription
    {
        get; 
        set;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BackchannelAuthenticationFailureEvent"/> class.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="error">The error.</param>
    /// <param name="description">The description.</param>
    public BackchannelAuthenticationFailureEvent(
        ValidatedBackchannelAuthenticationRequest? request,
        string error,
        string description)
        : this()
    {
        if (null != request)
        {
            ClientId = request.ClientId;
            ClientName = request.Client?.ClientName;
            Scopes = request.RequestedScopes?.ToSpaceSeparatedString();

            if (request.Subject?.Identity is { IsAuthenticated: true })
            {
                SubjectId = request.Subject?.GetSubjectId();
            }
        }

        Endpoint = Constants.EndpointNames.BackchannelAuthentication;
        Error = error;
        ErrorDescription = description;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BackchannelAuthenticationFailureEvent"/> class.
    /// </summary>
    /// <param name="result">The result.</param>
    public BackchannelAuthenticationFailureEvent(BackchannelAuthenticationRequestValidationResult result)
        : this()
    {
        if (null != result.ValidatedRequest)
        {
            ClientId = result.ValidatedRequest?.Client?.ClientId;
            ClientName = result.ValidatedRequest?.Client?.ClientName;
            Scopes = result.ValidatedRequest?.RequestedScopes?.ToSpaceSeparatedString();

            if (null != result.ValidatedRequest?.Subject && result.ValidatedRequest.Subject.Identity.IsAuthenticated)
            {
                SubjectId = result.ValidatedRequest?.Subject?.GetSubjectId();
            }
        }

        Endpoint = Constants.EndpointNames.BackchannelAuthentication;
        Error = result.Error;
        ErrorDescription = result.ErrorDescription;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BackchannelAuthenticationFailureEvent"/> class.
    /// </summary>
    private BackchannelAuthenticationFailureEvent()
        : base(EventCategories.BackchannelAuthentication,
            "Backchannel Authentication Failure",
            EventTypes.Failure,
            EventIds.BackchannelAuthenticationFailure)
    {
    }
}