using StoryBlog.Web.Microservices.Identity.Application.Storage;

namespace StoryBlog.Web.Microservices.Identity.Application.Stores;

/// <summary>
/// Interface for the validation key store
/// </summary>
public interface IValidationKeysStore
{
    /// <summary>
    /// Gets all validation keys.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<SecurityKeyInfo>> GetValidationKeysAsync();
}