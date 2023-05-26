using Microsoft.AspNetCore.Authentication;

namespace StoryBlog.Web.Microservices.Identity.Application.Services.KeyManagement;

/// <summary>
/// In-memory implementation of ISigningKeyStoreCache based on static variables.
/// This expects to be used as a singleton.
/// </summary>
public class InMemoryKeyStoreCache : ISigningKeyStoreCache
{
    private readonly object @lock = new();

    private readonly ISystemClock clock;
    private DateTime expires = DateTime.MinValue;
    private ICollection<KeyContainer>? cache;

    /// <summary>
    /// Constructor for InMemoryKeyStoreCache.
    /// </summary>
    /// <param name="clock"></param>
    public InMemoryKeyStoreCache(ISystemClock clock)
    {
        this.clock = clock;
    }

    /// <summary>
    /// Returns cached keys.
    /// </summary>
    /// <returns></returns>
    public Task<ICollection<KeyContainer>?> GetKeysAsync()
    {
        DateTime dateTime;
        ICollection<KeyContainer>? keys;

        lock (@lock)
        {
            dateTime = expires;
            keys = cache;
        }

        if (null != keys && dateTime >= clock.UtcNow.UtcDateTime)
        {
            return Task.FromResult<ICollection<KeyContainer>?>(keys);
        }

        return Task.FromResult<ICollection<KeyContainer>?>(null);
    }

    /// <summary>
    /// Caches keys for duration.
    /// </summary>
    /// <param name="keys"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public Task StoreKeysAsync(ICollection<KeyContainer> keys, TimeSpan duration)
    {
        lock (@lock)
        {
            expires = clock.UtcNow.UtcDateTime.Add(duration);
            cache = keys;
        }

        return Task.CompletedTask;
    }
}