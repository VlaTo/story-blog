using StoryBlog.Web.Microservices.Identity.Application.Storage;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation.Results;

/// <summary>
/// Validation result for API validation
/// </summary>
public class ApiSecretValidationResult : ValidationResult
{
    /// <summary>
    /// Gets or sets the resource.
    /// </summary>
    /// <value>
    /// The resource.
    /// </value>
    public ApiResource? Resource
    {
        get; 
        init;
    }
}