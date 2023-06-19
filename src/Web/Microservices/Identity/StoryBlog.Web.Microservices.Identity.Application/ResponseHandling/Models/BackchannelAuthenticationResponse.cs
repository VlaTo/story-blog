namespace StoryBlog.Web.Microservices.Identity.Application.ResponseHandling.Models;

/// <summary>
/// Models a backchannel authentication response
/// </summary>
public sealed class BackchannelAuthenticationResponse
{
    /// <summary>
    /// Indicates if this response represents an error.
    /// </summary>
    public bool IsError => false == String.IsNullOrWhiteSpace(Error);

    /// <summary>
    /// Gets or sets the error.
    /// </summary>
    public string? Error
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the error description.
    /// </summary>
    public string? ErrorDescription
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the authentication request id.
    /// </summary>
    public string AuthenticationRequestId
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the expires in.
    /// </summary>
    public TimeSpan ExpiresIn
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the interval.
    /// </summary>
    public TimeSpan Interval
    {
        get;
        set;
    }

    /// <summary>
    /// Ctor.
    /// </summary>
    public BackchannelAuthenticationResponse()
    {
    }

    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="error"></param>
    /// <param name="errorDescription"></param>
    public BackchannelAuthenticationResponse(string? error, string? errorDescription = null)
    {
        Error = error;
        ErrorDescription = errorDescription;
    }
}