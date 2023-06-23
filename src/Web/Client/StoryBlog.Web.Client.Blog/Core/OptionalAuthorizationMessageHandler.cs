using System.Net;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Headers;

namespace StoryBlog.Web.Client.Blog.Core;

internal class OptionalAuthorizationMessageHandler : DelegatingHandler, IDisposable
{
    private readonly IAccessTokenProvider provider;
    private AccessTokenRequestOptions? tokenOptions;
    private Uri[]? authorizeUrls;
    private DateTimeOffset? tokenTimeout;
    private AuthenticationHeaderValue? cachedHeader;

    public OptionalAuthorizationMessageHandler(IAccessTokenProvider provider)
    {
        this.provider = provider;

        if (provider is AuthenticationStateProvider stateProvider)
        {
            stateProvider.AuthenticationStateChanged += ResetAuthenticationState;
        }
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.Now;
        
        if (null == authorizeUrls)
        {
            var message = String.Format(NotConfiguredErrorMessage, nameof(OptionalAuthorizationMessageHandler), nameof(ConfigureHandler));
            throw new InvalidOperationException(message);
        }

        if (authorizeUrls.Any(uri => uri.IsBaseOf(request.RequestUri)))
        {
            if (null == tokenTimeout || now >= tokenTimeout)
            {
                var tokenResult = tokenOptions != null
                    ? await provider.RequestAccessToken(tokenOptions)
                    : await provider.RequestAccessToken();

                if (tokenResult.TryGetToken(out var token))
                {
                    tokenTimeout = token.Expires;
                    cachedHeader = new AuthenticationHeaderValue(AuthorizationScheme.Bearer, token.Value);
                }
                else
                {
                    tokenTimeout = now + TimeSpan.FromHours(1.0d);
                    cachedHeader = null;
                }
            }

            if (null != cachedHeader)
            {
                request.Headers.Authorization = cachedHeader;
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }

    protected void ConfigureHandler(
        IEnumerable<string> urlsToAuthorize,
        IEnumerable<string>? scopes = null,
        string? returnUrl = null)
    {
        if (null != authorizeUrls)
        {
            throw new InvalidOperationException("Handler already configured.");
        }

        if (null == urlsToAuthorize)
        {
            throw new ArgumentNullException(nameof(urlsToAuthorize));
        }

        var urls = urlsToAuthorize
            .Select(uri => new Uri(uri, UriKind.Absolute))
            .ToArray();

        if (0 == urls.Length)
        {
            throw new ArgumentException("At least one URL must be configured.", nameof(urlsToAuthorize));
        }

        authorizeUrls = urls;

        var scopesList = scopes?.ToArray();

        if (null != scopesList || null != returnUrl)
        {
            tokenOptions = new AccessTokenRequestOptions
            {
                Scopes = scopesList,
                ReturnUrl = returnUrl
            };
        }
    }

    private void ResetAuthenticationState(Task<AuthenticationState> task)
    {
        tokenTimeout = null;
    }

    void IDisposable.Dispose()
    {
        if (provider is AuthenticationStateProvider stateProvider)
        {
            stateProvider.AuthenticationStateChanged -= ResetAuthenticationState;
        }
        
        Dispose(true);
    }

    #region Messages

    private const string NotConfiguredErrorMessage = "The '{0}' is not configured. Call '{1}' and provide a list of endpoint urls to attach the token to.";

    #endregion
}