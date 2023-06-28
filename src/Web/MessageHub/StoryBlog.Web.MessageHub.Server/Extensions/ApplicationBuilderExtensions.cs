using Microsoft.AspNetCore.Builder;
using StoryBlog.Web.MessageHub.Server.Middlewares;

namespace StoryBlog.Web.MessageHub.Server.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseMessageHub(this IApplicationBuilder application)
    {
        application.UseMiddleware<WebSocketMessageHubMiddleware>();

        return application;
    }
}