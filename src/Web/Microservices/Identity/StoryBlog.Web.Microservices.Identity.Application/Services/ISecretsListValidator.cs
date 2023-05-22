using StoryBlog.Web.Microservices.Identity.Application.Models;
using StoryBlog.Web.Microservices.Identity.Application.Storage;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Results;

namespace StoryBlog.Web.Microservices.Identity.Application.Services;

/// <summary>
/// Validator for an Enumerable List of Secrets
/// </summary>
public interface ISecretsListValidator
{
    /// <summary>
    /// Validates a list of secrets
    /// </summary>
    /// <param name="secrets">The stored secrets.</param>
    /// <param name="parsedSecret">The received secret.</param>
    /// <returns>A validation result</returns>
    Task<SecretValidationResult> ValidateAsync(IEnumerable<Secret> secrets, ParsedSecret parsedSecret);
}