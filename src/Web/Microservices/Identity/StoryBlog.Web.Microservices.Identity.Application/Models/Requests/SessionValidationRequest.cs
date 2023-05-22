using StoryBlog.Web.Microservices.Identity.Application.Storage;

namespace StoryBlog.Web.Microservices.Identity.Application.Models.Requests;

/// <summary>
/// Represent the type of session validation request
/// </summary>
public enum SessionValidationType
{
    /// <summary>
    /// Refresh token use at token endpoint
    /// </summary>
    RefreshToken,

    /// <summary>
    /// Access token use by client at userinfo endpoint or at an API that uses introspection
    /// </summary>
    AccessToken
}

/// <summary>
/// Models request to validation a session from a client.
/// </summary>
public class SessionValidationRequest
{
    /// <summary>
    /// The subject ID
    /// </summary>
    public string SubjectId
    {
        get;
        set;
    }

    /// <summary>
    /// The session ID
    /// </summary>
    public string SessionId
    {
        get;
        set;
    }

    /// <summary>
    /// The client making the request.
    /// </summary>
    public Client Client
    {
        get;
        set;
    }

    /// <summary>
    /// Indicates the type of request.
    /// </summary>
    public SessionValidationType Type
    {
        get;
        set;
    }
}