namespace StoryBlog.Web.Microservices.Identity.Application.Services.KeyManagement;

/// <summary>
/// Interface to model caching keys loaded from key store.
/// </summary>
public interface ISigningKeyStoreCache
{
    /// <summary>
    /// Returns cached keys.
    /// </summary>
    /// <returns></returns>
    Task<ICollection<KeyContainer>?> GetKeysAsync();

    /// <summary>
    /// Caches keys for duration.
    /// </summary>
    /// <param name="keys"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    Task StoreKeysAsync(ICollection<KeyContainer> keys, TimeSpan duration);
}