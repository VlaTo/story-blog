using StoryBlog.Web.Microservices.Identity.Application.Validation.Contexts;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation;

/// <summary>
/// Allows inserting custom validation logic into authorize and token requests
/// </summary>
public interface ICustomAuthorizeRequestValidator
{
    /// <summary>
    /// Custom validation logic for the authorize request.
    /// </summary>
    /// <param name="context">The context.</param>
    Task ValidateAsync(CustomAuthorizeRequestValidationContext context);
}