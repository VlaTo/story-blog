using Microsoft.AspNetCore.Http;
using StoryBlog.Web.Microservices.Identity.Application.Models;

namespace StoryBlog.Web.Microservices.Identity.Application.Services;

/// <summary>
/// Parser for finding the best secret in an Enumerable List
/// </summary>
public interface ISecretsListParser
{
    /// <summary>
    /// Tries to find the best secret on the context that can be used for authentication
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>
    /// A parsed secret
    /// </returns>
    Task<ParsedSecret?> ParseAsync(HttpContext context);

    /// <summary>
    /// Gets all available authentication methods.
    /// </summary>
    /// <returns></returns>
    IEnumerable<string> GetAvailableAuthenticationMethods();
}