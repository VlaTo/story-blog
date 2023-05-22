using StoryBlog.Web.Microservices.Identity.Application.Validation.Contexts;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation.Defaults;

/// <summary>
/// Default custom request validator
/// </summary>
internal sealed class DefaultCustomTokenRequestValidator : ICustomTokenRequestValidator
{
    /// <summary>
    /// Custom validation logic for a token request.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>
    /// The validation result
    /// </returns>
    public Task ValidateAsync(CustomTokenRequestValidationContext context)
    {
        return Task.CompletedTask;
    }
}