using StoryBlog.Web.Microservices.Identity.Application.Validation.Contexts;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation;

/// <summary>
/// Validator for handling client authentication
/// </summary>
public interface IClientConfigurationValidator
{
    /// <summary>
    /// Determines whether the configuration of a client is valid.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    Task ValidateAsync(ClientConfigurationValidationContext context);
}