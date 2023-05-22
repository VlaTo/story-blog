using Microsoft.IdentityModel.Tokens;
using StoryBlog.Web.Microservices.Identity.Application.Stores;

namespace StoryBlog.Web.Microservices.Identity.Application.Services.KeyManagement;

/// <summary>
/// Store abstraction for automatic key management.
/// </summary>
public interface IAutomaticKeyManagerKeyStore : IValidationKeysStore, ISigningCredentialStore
{
    /// <summary>
    /// Gets all the signing credentials.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<SigningCredentials>> GetAllSigningCredentialsAsync();
}