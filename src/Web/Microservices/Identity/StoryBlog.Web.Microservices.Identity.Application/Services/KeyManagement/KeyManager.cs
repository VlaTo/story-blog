﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Microservices.Identity.Application.Configuration;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Application.Stores;

namespace StoryBlog.Web.Microservices.Identity.Application.Services.KeyManagement;

/// <summary>
/// Implementation of IKeyManager that creates, stores, and rotates signing keys.
/// </summary>
public class KeyManager : IKeyManager
{
    private readonly IdentityServerOptions options;
    private readonly ISigningKeyStore store;
    private readonly ISigningKeyStoreCache storeCache;
    private readonly ISigningKeyProtector protector;
    private readonly ISystemClock clock;
    private readonly IConcurrencyLock<KeyManager> newKeyLock;
    private readonly ILogger<KeyManager> logger;
    private readonly IIssuerNameService issuerNameService;

    /// <summary>
    /// Constructor for KeyManager
    /// </summary>
    /// <param name="options"></param>
    /// <param name="store"></param>
    /// <param name="storeCache"></param>
    /// <param name="protector"></param>
    /// <param name="clock"></param>
    /// <param name="newKeyLock"></param>
    /// <param name="logger"></param>
    /// <param name="issuerNameService"></param>
    public KeyManager(
        IdentityServerOptions options,
        ISigningKeyStore store,
        ISigningKeyStoreCache storeCache,
        ISigningKeyProtector protector,
        ISystemClock clock,
        IConcurrencyLock<KeyManager> newKeyLock,
        IIssuerNameService issuerNameService,
        ILogger<KeyManager> logger)
    {
        options.KeyManagement.Validate();

        this.options = options;
        this.store = store;
        this.storeCache = storeCache;
        this.protector = protector;
        this.clock = clock;
        this.newKeyLock = newKeyLock;
        this.logger = logger;
        this.issuerNameService = issuerNameService;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<KeyContainer>> GetCurrentKeysAsync()
    {
        using (Tracing.ActivitySource.StartActivity("KeyManageer.GetCurrentKeys"))
        {
            logger.LogDebug("Getting the current key.");

            var (_, currentKeys) = await GetAllKeysInternalAsync();

            if (logger.IsEnabled(LogLevel.Information))
            {
                foreach (var key in currentKeys)
                {
                    var age = clock.GetAge(key.Created);
                    var expiresIn = key.Created + options.KeyManagement.RotationInterval.Subtract(age);
                    var retiresIn = key.Created + options.KeyManagement.KeyRetirementAge.Subtract(age);
                    
                    logger.LogInformation(
                        "Active signing key found with kid {0} for alg {1}. Expires in {2}. Retires in {3}",
                        key.Id, key.Algorithm, expiresIn, retiresIn
                    );
                }
            }

            return currentKeys;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<KeyContainer>> GetAllKeysAsync()
    {
        using (Tracing.ActivitySource.StartActivity("KeyManager.GetAllKey"))
        {
            logger.LogDebug("Getting all the keys.");

            var (keys, _) = await GetAllKeysInternalAsync();

            return keys;
        }
    }

    internal async Task<(ICollection<KeyContainer> allKeys, ICollection<KeyContainer> signingKeys)> GetAllKeysInternalAsync()
    {
        var cached = true;
        var keys = await GetKeysFromCacheAsync();

        if (false == keys.Any())
        {
            cached = false;
            keys = await GetKeysFromStoreAsync();
        }

        // ensure we have all of our active signing keys
        var signingKeysSuccess = TryGetAllCurrentSigningKeys(keys, out var signingKeys);

        // if we loaded from cache, see if DB has updated key
        if (false == signingKeysSuccess && cached)
        {
            logger.LogDebug("Not all signing keys current in cache, reloading keys from database.");
        }

        var rotationRequired = false;

        // if we don't have an active key, then a new one is about to be created so don't bother running this check
        if (signingKeysSuccess)
        {
            rotationRequired = IsKeyRotationRequired(keys);
            if (rotationRequired && cached)
            {
                logger.LogDebug("Key rotation required, reloading keys from database.");
            }
        }

        if (false == signingKeysSuccess || rotationRequired)
        {
            logger.LogDebug("Entering new key lock.");

            // need to create new key, but another thread might have already so acquiring lock.
            if (false == await newKeyLock.LockAsync(options.Caching.CacheLockTimeout))
            {
                throw new Exception($"Failed to obtain new key lock for: '{GetType()}'");
            }

            try
            {
                // check if another thread did the work already
                keys = await GetKeysFromCacheAsync();

                if (false == signingKeysSuccess)
                {
                    signingKeysSuccess = TryGetAllCurrentSigningKeys(keys, out signingKeys);
                }
                if (rotationRequired)
                {
                    rotationRequired = IsKeyRotationRequired(keys);
                }

                if (false == signingKeysSuccess || rotationRequired)
                {
                    // still need to do the work, but check if another server did the work already
                    keys = await GetKeysFromStoreAsync();

                    if (false == signingKeysSuccess)
                    {
                        signingKeysSuccess = TryGetAllCurrentSigningKeys(keys, out signingKeys);
                    }
                    if (rotationRequired)
                    {
                        rotationRequired = IsKeyRotationRequired(keys);
                    }

                    if (false == signingKeysSuccess || rotationRequired)
                    {
                        if (false == signingKeysSuccess)
                        {
                            logger.LogDebug("No active keys; new key creation required.");
                        }
                        else
                        {
                            logger.LogDebug("Approaching key retirement; new key creation required.");
                        }

                        // now we know we need to create new keys
                        (keys, signingKeys) = await CreateNewKeysAndAddToCacheAsync();
                    }
                    else
                    {
                        logger.LogDebug("Another server created new key.");
                    }
                }
                else
                {
                    logger.LogDebug("Another thread created new key.");
                }
            }
            finally
            {
                logger.LogDebug("Releasing new key lock.");
                newKeyLock.Unlock();
            }
        }

        if (false == signingKeys.Any())
        {
            logger.LogError("Failed to create and then load new keys.");
            throw new Exception("Failed to create and then load new keys.");
        }

        return (keys, signingKeys);
    }

    internal bool IsKeyRotationRequired(IEnumerable<KeyContainer>? allKeys)
    {
        if (null == allKeys || !allKeys.Any())
        {
            return true;
        }

        var groupedKeys = allKeys.GroupBy(x => x.Algorithm);
        var success = groupedKeys.Count() == options.KeyManagement.AllowedSigningAlgorithmNames.Count() &&
                      groupedKeys.All(x => options.KeyManagement.AllowedSigningAlgorithmNames.Contains(x.Key));

        if (!success)
        {
            return true;
        }

        foreach (var item in groupedKeys)
        {
            var keys = item.AsEnumerable();
            var activeKey = GetCurrentSigningKey(keys);

            if (null == activeKey)
            {
                return true;
            }

            // rotation is needed if: 1) if there are no other keys next in line (meaning younger).
            // and 2) the current activation key is near expiration (using the delay timeout)

            // get younger keys (which will also filter active key)
            keys = keys.Where(x => x.Created > activeKey.Created).ToArray();

            if (keys.Any())
            {
                // there are younger keys, then they might also be within the window of the key activation delay
                // so find the youngest one and treat that one as if it's the active key.
                activeKey = keys.OrderByDescending(x => x.Created).First();
            }

            // if no younger keys, then check if we're nearing the expiration of active key
            // and see if that's within the window of activation delay.
            var age = clock.GetAge(activeKey.Created);
            var diff = options.KeyManagement.RotationInterval.Subtract(age);
            var needed = (diff <= options.KeyManagement.PropagationTime);

            if (!needed)
            {
                logger.LogDebug("Key rotation not required for alg {alg}; New key expected to be created in {KeyRotiation}", item.Key, diff.Subtract(options.KeyManagement.PropagationTime));
            }
            else
            {
                logger.LogDebug("Key rotation required now for alg {alg}.", item.Key);
                return true;
            }
        }

        return false;
    }

    internal async Task<KeyContainer> CreateAndStoreNewKeyAsync(SigningAlgorithmOptions alg)
    {
        logger.LogDebug("Creating new key.");

        var now = clock.UtcNow.UtcDateTime;
        var iss = await issuerNameService.GetCurrentAsync();

        KeyContainer? container = null;

        if (alg.IsRsaKey)
        {
            var rsa = CryptoHelper.CreateRsaSecurityKey(options.KeyManagement.RsaKeySize);

            container = alg.UseX509Certificate
                ? new X509KeyContainer(rsa, alg.Name, now, options.KeyManagement.KeyRetirementAge, iss)
                : new RsaKeyContainer(rsa, alg.Name, now);
        }
        else if (alg.IsEcKey)
        {
            var curveName = CryptoHelper.GetCurveNameFromSigningAlgorithm(alg.Name);
            var ec = CryptoHelper.CreateECDsaSecurityKey(curveName);
            // X509 certs don't currently work with EC keys.
            container = new EcKeyContainer(ec, alg.Name, now)
                //_options.KeyManagement.WrapKeysInX509Certificate ? //new X509KeyContainer(ec, alg, now, _options.KeyManagement.KeyRetirementAge, iss) :
                ;
        }
        else
        {
            throw new Exception($"Invalid alg '{alg}'");
        }

        var key = protector.Protect(container);

        await store.StoreKeyAsync(key);

        logger.LogInformation("Created and stored new key with kid {kid}.", container.Id);

        return container;
    }

    internal async Task<ICollection<KeyContainer>> GetKeysFromCacheAsync()
    {
        var cachedKeys = await storeCache.GetKeysAsync();

        if (null != cachedKeys)
        {
            logger.LogDebug("Cache hit when loading all keys.");
            return cachedKeys;
        }

        logger.LogDebug("Cache miss when loading all keys.");

        return Array.Empty<KeyContainer>();
    }

    internal bool AreAllKeysWithinInitializationDuration(IEnumerable<KeyContainer> keys)
    {
        if (options.KeyManagement.InitializationDuration == TimeSpan.Zero)
        {
            return false;
        }

        // the expired check will also filter retired keys
        keys = FilterExpiredKeys(keys);

        var result = keys.All(x =>
        {
            var age = clock.GetAge(x.Created);
            var isNew = options.KeyManagement.IsWithinInitializationDuration(age);

            return isNew;
        });

        return result;
    }

    internal async Task<ICollection<KeyContainer>> FilterAndDeleteRetiredKeysAsync(ICollection<KeyContainer> keys)
    {
        var retired = keys
            .Where(x =>
            {
                var age = clock.GetAge(x.Created);
                var isRetired = options.KeyManagement.IsRetired(age);
                return isRetired;
            })
            .ToArray();

        if (retired.Any())
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                var ids = retired.Select(x => x.Id).ToArray();
                logger.LogTrace("Filtered retired keys from store: {kids}", ids.Aggregate((x, y) => $"{x},{y}"));
            }

            if (options.KeyManagement.DeleteRetiredKeys)
            {
                var ids = retired.Select(x => x.Id).ToArray();
                if (logger.IsEnabled(LogLevel.Debug))
                {
                    logger.LogDebug("Deleting retired keys from store: {kids}", ids.Aggregate((x, y) => $"{x},{y}"));
                }
                await DeleteKeysAsync(ids);
            }
        }

        var result = keys.Except(retired).ToArray();

        return result;
    }

    internal async Task DeleteKeysAsync(IEnumerable<string> keys)
    {
        if (keys == null || !keys.Any()) return;

        foreach (var key in keys)
        {
            await store.DeleteKeyAsync(key);
        }
    }

    internal IEnumerable<KeyContainer> FilterExpiredKeys(IEnumerable<KeyContainer> keys)
    {
        var result = keys
            .Where(x =>
            {
                var age = clock.GetAge(x.Created);
                var isExpired = options.KeyManagement.IsExpired(age);

                return false == isExpired;
            });

        return result;
    }

    internal async Task CacheKeysAsync(ICollection<KeyContainer> keys)
    {
        if (false == keys.Any())
        {
            return ;
        }

        var duration = options.KeyManagement.KeyCacheDuration;

        if (AreAllKeysWithinInitializationDuration(keys))
        {
            // if all key are new, then we want to use the shorter initialization key cache duration.
            // this attempts to allow other servers that are slow to write new keys to complete, then we will
            // have the most up to date keys in the cache sooner.
            duration = options.KeyManagement.InitializationKeyCacheDuration;
            
            if (duration > TimeSpan.Zero)
            {
                logger.LogDebug(
                    "Caching keys with InitializationKeyCacheDuration for {InitializationKeyCacheDuration}",
                    options.KeyManagement.InitializationKeyCacheDuration
                );
            }
        }
        else if (options.KeyManagement.KeyCacheDuration > TimeSpan.Zero)
        {
            logger.LogDebug(
                "Caching keys with KeyCacheDuration for {KeyCacheDuration}",
                options.KeyManagement.KeyCacheDuration
            );
        }

        if (TimeSpan.Zero < duration)
        {
            await storeCache.StoreKeysAsync(keys, duration);
        }
    }

    internal async Task<ICollection<KeyContainer>> GetKeysFromStoreAsync(bool cache = true)
    {
        logger.LogDebug("Loading keys from store.");

        var protectedKeys = await store.LoadKeysAsync();

        if (protectedKeys.Any())
        {
            ICollection<KeyContainer?> keys = protectedKeys
                .Select(x =>
                {
                    try
                    {
                        var key = protector.Unprotect(x);

                        if (null == key)
                        {
                            logger.LogWarning("Key with kid {kid} failed to unprotect.", x.Id);
                        }

                        return key;
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error unprotecting key with kid {kid}.", x?.Id);
                    }

                    return null;
                })
                .Where(x => x != null)
                .ToArray();

            if (logger.IsEnabled(LogLevel.Trace) && keys.Any())
            {
                var ids = keys.Select(x => x.Id).ToArray();
                logger.LogTrace("Loaded keys from store: {kids}", ids.Aggregate((x, y) => $"{x},{y}"));
            }

            // retired keys are those that are beyond inclusion, thus we act as if they don't exist.
            keys = await FilterAndDeleteRetiredKeysAsync(keys);

            if (logger.IsEnabled(LogLevel.Trace) && keys.Any())
            {
                var ids = keys.Select(x => x.Id).ToArray();
                logger.LogTrace("Remaining keys after filter: {kids}", ids.Aggregate((x, y) => $"{x},{y}"));
            }

            // only use keys that are allowed
            keys = keys.Where(x => options.KeyManagement.AllowedSigningAlgorithmNames.Contains(x.Algorithm)).ToArray();
            if (logger.IsEnabled(LogLevel.Trace) && keys.Any())
            {
                var ids = keys.Select(x => x.Id).ToArray();
                logger.LogTrace("Keys with allowed alg from store: {kids}", ids.Aggregate((x, y) => $"{x},{y}"));
            }

            if (keys.Any())
            {
                logger.LogDebug("Keys successfully returned from store.");

                if (cache)
                {
                    await CacheKeysAsync(keys);
                }

                return keys;
            }
        }

        logger.LogInformation("No keys returned from store.");

        return Array.Empty<KeyContainer>();
    }
    
    internal async Task<(ICollection<KeyContainer> allKeys, ICollection<KeyContainer> activeKeys)> CreateNewKeysAndAddToCacheAsync()
    {
        var keys = new List<KeyContainer>();

        keys.AddRange(await storeCache.GetKeysAsync() ?? Enumerable.Empty<KeyContainer>());

        foreach (var alg in options.KeyManagement.SigningAlgorithms)
        {
            var newKey = await CreateAndStoreNewKeyAsync(alg);
            keys.Add(newKey);
        }

        if (AreAllKeysWithinInitializationDuration(keys))
        {
            // this is meant to allow multiple servers that all start at the same time to have some 
            // time to complete writing their newly created keys to the store. then when all load
            // each other's keys, they should all agree on the oldest key based on created time.
            // it's intended to address the scenario where two servers start, server1 creates a key whose
            // time is earlier than server2, but server1 is slow to write the key to the store.
            // we don't want server2 to only see server2's key, as it's newer.
            if (options.KeyManagement.InitializationSynchronizationDelay > TimeSpan.Zero)
            {
                logger.LogDebug("All keys are new; delaying before reloading keys from store by InitializationSynchronizationDelay for {InitializationSynchronizationDelay}.", options.KeyManagement.InitializationSynchronizationDelay);
                await Task.Delay(options.KeyManagement.InitializationSynchronizationDelay);
            }
            else
            {
                logger.LogDebug("All keys are new; reloading keys from store.");
            }

            // reload in case other new keys were recently created
            keys = new List<KeyContainer>(await GetKeysFromStoreAsync(false));
        }

        // explicitly cache here since we didn't when we loaded above
        await CacheKeysAsync(keys);

        var activeKeys = GetCurrentSigningKeys(keys);

        return (keys, activeKeys);
    }

    internal bool TryGetAllCurrentSigningKeys(ICollection<KeyContainer> keys, out ICollection<KeyContainer> signingKeys)
    {
        signingKeys = GetCurrentSigningKeys(keys);

        var success = signingKeys.Count == options.KeyManagement.AllowedSigningAlgorithmNames.Count() &&
                      signingKeys.All(x => options.KeyManagement.AllowedSigningAlgorithmNames.Contains(x.Algorithm));

        return success;
    }

    internal ICollection<KeyContainer> GetCurrentSigningKeys(ICollection<KeyContainer>? keys)
    {
        if (null == keys || false == keys.Any())
        {
            return Array.Empty<KeyContainer>();
        }

        logger.LogDebug("Looking for active signing keys.");

        var list = new List<KeyContainer>();
        var groupedKeys = keys.GroupBy(x => x.Algorithm);
        
        foreach (var item in groupedKeys)
        {
            logger.LogDebug("Looking for an active signing key for alg {alg}.", item.Key);

            var activeKey = GetCurrentSigningKey(item);
            
            if (null != activeKey)
            {
                logger.LogDebug("Found active signing key for alg {alg} with kid {kid}.", item.Key, activeKey.Id);
                list.Add(activeKey);
            }
            else
            {
                logger.LogDebug("Failed to find active signing key for alg {alg}.", item.Key);
            }
        }

        return list;
    }

    internal KeyContainer? GetCurrentSigningKey(IEnumerable<KeyContainer>? keys)
    {
        if (null == keys || false == keys.Any())
        {
            return null;
        }

        var ignoreActivation = false;
        // look for keys past activity delay
        var activeKey = GetCurrentSigningKeyInternal(keys, ignoreActivation);

        if (activeKey == null)
        {
            ignoreActivation = true;
            logger.LogDebug("No active signing key found (respecting the activation delay).");

            // none, so check if any of the keys were recently created
            activeKey = GetCurrentSigningKeyInternal(keys, ignoreActivation);

            if (activeKey == null)
            {
                logger.LogDebug("No active signing key found (ignoring the activation delay).");
            }
        }

        if (activeKey != null && logger.IsEnabled(LogLevel.Debug))
        {
            var delay = ignoreActivation ? "(ignoring the activation delay)" : "(respecting the activation delay)";
            logger.LogDebug("Active signing key found " + delay + " with kid: {kid}.", activeKey.Id);
        }

        return activeKey;
    }

    internal KeyContainer? GetCurrentSigningKeyInternal(IEnumerable<KeyContainer>? keys, bool ignoreActivationDelay = false)
    {
        if (null == keys)
        {
            return null;
        }

        keys = keys
            .Where(key => CanBeUsedAsCurrentSigningKey(key, ignoreActivationDelay))
            .ToArray();

        if (false == keys.Any())
        {
            return null;
        }

        // we order by the created date, in essence loading the oldest key
        // this accomodates the scenario where 2 servers create keys at the same time
        // but the first server only reloads the one key it created (and only has the one key for 
        // discovery). we don't want the second server using a key that's not in the first server's
        // discovery document. this will be somewhat mitigated by the initial duration where we 
        // deliberatly ignore the cache.
        var result = keys
            .OrderBy(x => x.Created)
            .First();

        return result;
    }

    internal bool CanBeUsedAsCurrentSigningKey(KeyContainer? key, bool ignoreActiveDelay = false)
    {
        if (null == key)
        {
            return false;
        }

        var alg = options.KeyManagement.SigningAlgorithms.SingleOrDefault(x => x.Name == key.Algorithm);

        if (null == alg)
        {
            logger.LogTrace("Key {kid} signing algorithm {alg} not allowed by server options.", key.Id, key.Algorithm);
            return false;
        }

        if (alg.UseX509Certificate && false == key.HasX509Certificate)
        {
            logger.LogTrace("Server configured to wrap keys in X509 certs, but key {kid} is not wrapped in cert.", key.Id);
            return false;
        }

        var now = clock.UtcNow.UtcDateTime;

        // newly created key check
        var start = key.Created;

        if (start > now)
        {
            // if another server created the key in the future (meaning this server's clock is 
            // behind the other), then we will just assume the other server's time for this key. 
            // this is how we can deal with clock skew for recently created keys. 
            now = start;
        }

        if (false == ignoreActiveDelay)
        {
            logger.LogTrace("Checking if key with kid {kid} is active (respecting activation delay).", key.Id);
            start = start.Add(options.KeyManagement.PropagationTime);
        }
        else
        {
            logger.LogTrace("Checking if key with kid {kid} is active (ignoring activation delay).", key.Id);
        }

        if (start > now)
        {
            logger.LogTrace("Key with kid {kid} is inactive: the current time is prior to its activation delay.", key.Id);
            return false;
        }

        // expired key check
        var end = key.Created.Add(options.KeyManagement.RotationInterval);

        if (end < now)
        {
            logger.LogTrace("Key with kid {kid} is inactive: the current time is past its expiration.", key.Id);
            return false;
        }

        logger.LogTrace("Key with kid {kid} is active.", key.Id);

        return true;
    }
}