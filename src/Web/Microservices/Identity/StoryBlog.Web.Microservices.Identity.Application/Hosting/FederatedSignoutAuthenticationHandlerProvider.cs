using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Hosting.FederatedSignOut;

namespace StoryBlog.Web.Microservices.Identity.Application.Hosting;

internal sealed class FederatedSignoutAuthenticationHandlerProvider : IAuthenticationHandlerProvider
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IAuthenticationHandlerProvider provider;

    public FederatedSignoutAuthenticationHandlerProvider(
        Decorator<IAuthenticationHandlerProvider> decorator,
        IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
        provider = decorator.Instance;
    }

    public async Task<IAuthenticationHandler?> GetHandlerAsync(HttpContext context, string authenticationScheme)
    {
        var handler = await provider.GetHandlerAsync(context, authenticationScheme);

        if (handler is IAuthenticationRequestHandler requestHandler)
        {
            if (requestHandler is IAuthenticationSignInHandler signinHandler)
            {
                return new AuthenticationRequestSignInHandlerWrapper(signinHandler, httpContextAccessor);
            }

            if (requestHandler is IAuthenticationSignOutHandler signOutHandler)
            {
                return new AuthenticationRequestSignOutHandlerWrapper(signOutHandler, httpContextAccessor);
            }

            return new AuthenticationRequestHandlerWrapper(requestHandler, httpContextAccessor);
        }

        return handler;
    }
}