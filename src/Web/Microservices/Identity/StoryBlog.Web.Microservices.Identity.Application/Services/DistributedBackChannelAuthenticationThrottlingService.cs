using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Distributed;
using StoryBlog.Web.Microservices.Identity.Application.Configuration;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Application.Stores;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Requests;

namespace StoryBlog.Web.Microservices.Identity.Application.Services;

/// <summary>
/// Implementation of IBackchannelAuthenticationThrottlingService that uses the IDistributedCache.
/// </summary>
public class DistributedBackChannelAuthenticationThrottlingService : IBackChannelAuthenticationThrottlingService
{
    private readonly IDistributedCache cache;
    private readonly IClientStore clientStore;
    private readonly ISystemClock clock;
    private readonly IdentityServerOptions options;

    private const string KeyPrefix = "backchannel_";

    /// <summary>
    /// Ctor
    /// </summary>
    public DistributedBackChannelAuthenticationThrottlingService(
        IDistributedCache cache,
        IClientStore clientStore,
        ISystemClock clock,
        IdentityServerOptions options)
    {
        this.cache = cache;
        this.clientStore = clientStore;
        this.clock = clock;
        this.options = options;
    }

    /// <inheritdoc/>
    public async Task<bool> ShouldSlowDown(string requestId, BackChannelAuthenticationRequest details)
    {
        using var activity = Tracing.ServiceActivitySource.StartActivity("DistributedBackchannelAuthenticationThrottlingService.ShouldSlowDown");

        if (null == requestId)
        {
            throw new ArgumentNullException(nameof(requestId));
        }

        var key = KeyPrefix + requestId;
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = clock.UtcNow + details.Lifetime
        };

        var lastSeenAsString = await cache.GetStringAsync(key);

        // record new
        if (null == lastSeenAsString)
        {
            await cache.SetStringAsync(key, clock.UtcNow.ToString("O"), cacheOptions);
            return false;
        }

        // check interval
        if (DateTime.TryParse(lastSeenAsString, out var lastSeen))
        {
            lastSeen = lastSeen.ToUniversalTime();

            var client = await clientStore.FindEnabledClientByIdAsync(details.ClientId);
            var interval = client?.PollingInterval ?? options.Ciba.DefaultPollingInterval;

            if ((lastSeen + interval) > clock.UtcNow.UtcDateTime)
            {
                await cache.SetStringAsync(key, clock.UtcNow.ToString("O"), cacheOptions);
                return true;
            }
        }

        // store current and continue
        await cache.SetStringAsync(key, clock.UtcNow.ToString("O"), cacheOptions);

        return false;
    }
}