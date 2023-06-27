using Microsoft.AspNetCore.Builder;
using StoryBlog.Web.Hub.Middlewares;

namespace StoryBlog.Web.Hub.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseMessageHub(this IApplicationBuilder application)
    {
        application.UseMiddleware<WebSocketMessageHubMiddleware>();

        return application;
    }
}