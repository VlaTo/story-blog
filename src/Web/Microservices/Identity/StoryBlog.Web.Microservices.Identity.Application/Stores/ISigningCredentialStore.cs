using Microsoft.IdentityModel.Tokens;

namespace StoryBlog.Web.Microservices.Identity.Application.Stores;

/// <summary>
/// Interface for a signing credential store
/// </summary>
public interface ISigningCredentialStore
{
    /// <summary>
    /// Gets the signing credentials.
    /// </summary>
    /// <returns></returns>
    Task<SigningCredentials?> GetSigningCredentialsAsync();
}