using Microsoft.AspNetCore.Http;
using StoryBlog.Web.Microservices.Identity.Application.Models;

namespace StoryBlog.Web.Microservices.Identity.Application.Services;

/// <summary>
/// A service for parsing secrets found on the request
/// </summary>
public interface ISecretParser
{
    /// <summary>
    /// Returns the authentication method name that this parser implements
    /// </summary>
    /// <value>
    /// The authentication method.
    /// </value>
    string AuthenticationMethod
    {
        get;
    }

    /// <summary>
    /// Tries to find a secret on the context that can be used for authentication
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>
    /// A parsed secret
    /// </returns>
    Task<ParsedSecret?> ParseAsync(HttpContext context);
}