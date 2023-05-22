using StoryBlog.Web.Microservices.Identity.Application.Models.Requests;
using StoryBlog.Web.Microservices.Identity.Application.Storage;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Results;

namespace StoryBlog.Web.Microservices.Identity.Application.Services;

/// <summary>
/// Implements refresh token creation and validation
/// </summary>
public interface IRefreshTokenService
{
    /// <summary>
    /// Validates a refresh token.
    /// </summary>
    /// <param name="token">The refresh token.</param>
    /// <param name="client">The client.</param>
    /// <returns></returns>
    Task<TokenValidationResult> ValidateRefreshTokenAsync(string token, Client client);

    /// <summary>
    /// Creates the refresh token.
    /// </summary>
    /// <returns>
    /// The refresh token handle
    /// </returns>
    Task<string> CreateRefreshTokenAsync(RefreshTokenCreationRequest request);

    /// <summary>
    /// Updates the refresh token.
    /// </summary>
    /// <returns>
    /// The refresh token handle
    /// </returns>
    Task<string> UpdateRefreshTokenAsync(RefreshTokenUpdateRequest request);
}