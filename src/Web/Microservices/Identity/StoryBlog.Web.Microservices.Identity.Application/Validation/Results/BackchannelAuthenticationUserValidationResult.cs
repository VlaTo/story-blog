using System.Security.Claims;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation.Results;

/// <summary>
/// Represents the result of a backchannel authentication request.
/// </summary>
public class BackchannelAuthenticationUserValidationResult : ValidationResult
{
    /// <summary>
    /// Gets or sets the subject based upon the provided hint.
    /// </summary>
    public ClaimsPrincipal? Subject
    {
        get;
        set;
    }
}