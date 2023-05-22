using StoryBlog.Web.Microservices.Identity.Application.Validation.Contexts;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Results;

namespace StoryBlog.Web.Microservices.Identity.Application.Services;

/// <summary>
/// Interface for request object validator
/// </summary>
public interface IJwtRequestValidator
{
    /// <summary>
    /// Validates a JWT request object
    /// </summary>
    Task<JwtRequestValidationResult> ValidateAsync(JwtRequestValidationContext context);
}