using Microsoft.AspNetCore.Http;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Results;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation;

/// <summary>
/// Validator for handling API client authentication.
/// </summary>
public interface IApiSecretValidator
{
    /// <summary>
    /// Tries to authenticate an API client based on the incoming request
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    Task<ApiSecretValidationResult> ValidateAsync(HttpContext context);
}