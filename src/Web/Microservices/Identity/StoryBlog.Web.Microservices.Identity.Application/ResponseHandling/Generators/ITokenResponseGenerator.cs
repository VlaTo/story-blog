using StoryBlog.Web.Microservices.Identity.Application.ResponseHandling.Models;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Results;

namespace StoryBlog.Web.Microservices.Identity.Application.ResponseHandling.Generators;

/// <summary>
/// Interface the token response generator
/// </summary>
public interface ITokenResponseGenerator
{
    /// <summary>
    /// Processes the response.
    /// </summary>
    /// <param name="validationResult">The validation result.</param>
    /// <returns></returns>
    Task<TokenResponse> ProcessAsync(TokenRequestValidationResult validationResult);
}