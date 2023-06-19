using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace StoryBlog.Web.Client.Blog.Core;

internal sealed class BlogApiAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public BlogApiAuthorizationMessageHandler(
        IAccessTokenProvider provider,
        NavigationManager navigation)
        : base(provider, navigation)
    {
        ConfigureHandler(
            authorizedUrls: new[]
            {
                "http://localhost:5033"
            },
            scopes: new[]
            {
                "openid",
                "profile",
                "blog"
            }
        );
    }
}