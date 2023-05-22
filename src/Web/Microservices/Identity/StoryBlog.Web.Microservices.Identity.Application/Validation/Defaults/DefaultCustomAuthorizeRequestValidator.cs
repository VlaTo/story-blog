using StoryBlog.Web.Microservices.Identity.Application.Validation.Contexts;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation.Defaults;

/// <summary>
/// Default custom request validator
/// </summary>
public class DefaultCustomAuthorizeRequestValidator : ICustomAuthorizeRequestValidator
{
    /// <summary>
    /// Custom validation logic for the authorize request.
    /// </summary>
    /// <param name="context">The context.</param>
    public Task ValidateAsync(CustomAuthorizeRequestValidationContext context)
    {
        return Task.CompletedTask;
    }
}