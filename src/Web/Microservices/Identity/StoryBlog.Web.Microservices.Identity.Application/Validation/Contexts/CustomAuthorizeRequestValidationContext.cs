using StoryBlog.Web.Microservices.Identity.Application.Validation.Results;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation.Contexts;

/// <summary>
/// Context for custom authorize request validation.
/// </summary>
public sealed class CustomAuthorizeRequestValidationContext
{
    /// <summary>
    /// The result of custom validation. 
    /// </summary>
    public AuthorizeRequestValidationResult Result
    {
        get;
        init;
    }
}