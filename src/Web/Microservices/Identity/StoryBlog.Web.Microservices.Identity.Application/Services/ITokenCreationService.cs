using StoryBlog.Web.Microservices.Identity.Application.Storage;

namespace StoryBlog.Web.Microservices.Identity.Application.Services;

/// <summary>
/// Logic for creating security tokens
/// </summary>
public interface ITokenCreationService
{
    /// <summary>
    /// Creates a token.
    /// </summary>
    /// <param name="token">The token description.</param>
    /// <returns>A protected and serialized security token</returns>
    Task<string> CreateTokenAsync(Token token);
}