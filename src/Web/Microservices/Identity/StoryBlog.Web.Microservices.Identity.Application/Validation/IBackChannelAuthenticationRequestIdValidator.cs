using StoryBlog.Web.Microservices.Identity.Application.Validation.Contexts;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation;

/// <summary>
/// The backchannel authentication request id validator
/// </summary>
public interface IBackChannelAuthenticationRequestIdValidator
{
    /// <summary>
    /// Validates the authentication request id.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    Task ValidateAsync(BackchannelAuthenticationRequestIdValidationContext context);
}