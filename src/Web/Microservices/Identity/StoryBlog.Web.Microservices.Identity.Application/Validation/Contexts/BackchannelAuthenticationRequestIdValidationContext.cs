using StoryBlog.Web.Microservices.Identity.Application.Validation.Requests;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Results;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation.Contexts;

/// <summary>
/// Context for backchannel authentication request id validation.
/// </summary>
public class BackchannelAuthenticationRequestIdValidationContext
{
    /// <summary>
    /// Gets or sets the authentication request id.
    /// </summary>
    /// <value>
    /// The device code.
    /// </value>
    public string AuthenticationRequestId
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the request.
    /// </summary>
    /// <value>
    /// The request.
    /// </value>
    public ValidatedTokenRequest Request
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the result.
    /// </summary>
    /// <value>
    /// The result.
    /// </value>
    public TokenRequestValidationResult Result
    {
        get;
        set;
    }
}