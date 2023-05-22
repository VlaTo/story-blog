using StoryBlog.Web.Microservices.Identity.Application.Validation.Contexts;

namespace StoryBlog.Web.Microservices.Identity.Application.Services;

/// <summary>
/// Handles validation of token requests using custom grant types
/// </summary>
public interface IExtensionGrantValidator
{
    /// <summary>
    /// Returns the grant type this validator can deal with
    /// </summary>
    /// <value>
    /// The type of the grant.
    /// </value>
    string GrantType
    {
        get;
    }

    /// <summary>
    /// Validates the custom grant request.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>
    /// A principal
    /// </returns>
    Task ValidateAsync(ExtensionGrantValidationContext context);
}