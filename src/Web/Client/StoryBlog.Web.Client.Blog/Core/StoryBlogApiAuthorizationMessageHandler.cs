using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;
using StoryBlog.Web.Client.Blog.Configuration;

namespace StoryBlog.Web.Client.Blog.Core;

internal sealed class StoryBlogApiAuthorizationMessageHandler : OptionalAuthorizationMessageHandler
{
    public StoryBlogApiAuthorizationMessageHandler(IAccessTokenProvider provider, IOptions<HttpClientOptions> options)
        : base(provider)
    {
        var endpoints = options.Value.Endpoints;

        ConfigureHandler(
            urlsToAuthorize: new[]
            {
                endpoints.Post.BasePath,
                endpoints.Posts.BasePath,
                endpoints.Comments.BasePath
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