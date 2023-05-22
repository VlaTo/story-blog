namespace StoryBlog.Web.Microservices.Identity.Application.Models.Requests;

/// <summary>
/// Information necessary to make a back-channel logout request to a client.
/// </summary>
public class BackChannelLogoutRequest
{
    /// <summary>
    /// Gets or sets the client identifier.
    /// </summary>
    public string ClientId
    {
        get;
        set;
    }

    /// <summary>
    /// Gets the subject identifier.
    /// </summary>
    public string SubjectId
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the session identifier.
    /// </summary>
    public string SessionId
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the back channel logout URI.
    /// </summary>
    public string LogoutUri
    {
        get;
        set;
    }

    /// <summary>
    /// Gets a value indicating whether the session identifier is required.
    /// </summary>
    public bool SessionIdRequired
    {
        get;
        set;
    }

    /// <summary>
    /// The issuer for the back-channel logout
    /// </summary>
    public string Issuer
    {
        get;
        set;
    }
}