using StoryBlog.Web.Microservices.Identity.Application.Validation.Contexts;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Results;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation;

/// <summary>
/// Interface for the backchannel authentication user validation
/// </summary>
public interface IBackchannelAuthenticationUserValidator
{
    /// <summary>
    /// Validates the user.
    /// </summary>
    /// <param name="userValidatorContext"></param>
    /// <returns></returns>
    Task<BackchannelAuthenticationUserValidationResult> ValidateRequestAsync(BackchannelAuthenticationUserValidatorContext userValidatorContext);
}