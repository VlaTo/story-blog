using StoryBlog.Web.Microservices.Identity.Application.ResponseHandling.Models;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Results;

namespace StoryBlog.Web.Microservices.Identity.Application.ResponseHandling.Generators;

/// <summary>
/// Interface the backchannel authentication response generator
/// </summary>
public interface IBackchannelAuthenticationResponseGenerator
{
    /// <summary>
    /// Processes the response.
    /// </summary>
    /// <param name="validationResult">The validation result.</param>
    /// <returns></returns>
    Task<BackchannelAuthenticationResponse> ProcessAsync(BackchannelAuthenticationRequestValidationResult validationResult);
}