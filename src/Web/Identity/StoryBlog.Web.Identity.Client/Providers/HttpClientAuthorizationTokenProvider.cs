using Microsoft.Extensions.Caching.Memory;
using StoryBlog.Web.Identity.Client.Extensions;
using StoryBlog.Web.Identity.Client.Requests;

namespace StoryBlog.Web.Identity.Client.Providers;

internal sealed class HttpClientAuthorizationTokenProvider(
    IHttpClientFactory httpClientFactory,
    IMemoryCache memoryCache)
    : IAuthorizationTokenProvider
{
    public async Task<AuthorizationToken?> GetAuthorizationTokenAsync(CancellationToken cancellationToken)
    {
        const string cacheKey = "AuthenticationToken";

        if (!memoryCache.TryGetValue(cacheKey, out AuthorizationTokenCacheEntry? entry))
        {
            using (var httpClient = httpClientFactory.CreateClient())
            {
                var disco = await httpClient.GetDiscoveryDocumentAsync("http://localhost:5030/", cancellationToken);

                var response = await httpClient.RequestClientCredentialsTokenAsync(
                    new ClientCredentialsTokenRequest
                    {
                        Address = disco.TokenEndpoint,
                        ClientId = "09b7b9b3496a42299123ce88c5429d96",
                        ClientSecret = "*mRo83cvyTz2aZ!zm2p^89d6JZe2sMf*dNSxRv",
                        Scope = "blog"
                    },
                    cancellationToken
                );

                if (response.IsError)
                {
                    throw new Exception();
                }

                entry = new AuthorizationTokenCacheEntry(response.AccessToken!);

                memoryCache.Set(cacheKey, entry);
            }
        }

        return new AuthorizationToken
        {
            Token = entry!.Token
        };
    }

    #region AuthorizationTikenCacheEntry

    private sealed record AuthorizationTokenCacheEntry(string Token);

    #endregion
}