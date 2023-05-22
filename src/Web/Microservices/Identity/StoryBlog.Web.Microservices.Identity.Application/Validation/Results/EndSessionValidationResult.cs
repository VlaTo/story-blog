using StoryBlog.Web.Microservices.Identity.Application.Validation.Requests;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation.Results;

/// <summary>
/// Validation result for end session requests
/// </summary>
/// <seealso cref="ValidationResult" />
public sealed class EndSessionValidationResult : ValidationResult
{
    /// <summary>
    /// Gets or sets the validated request.
    /// </summary>
    /// <value>
    /// The validated request.
    /// </value>
    public ValidatedEndSessionRequest ValidatedRequest
    {
        get;
        set;
    }
}