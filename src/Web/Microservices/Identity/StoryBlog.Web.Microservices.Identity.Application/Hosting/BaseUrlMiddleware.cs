using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using StoryBlog.Web.Microservices.Identity.Application.Services;

namespace StoryBlog.Web.Microservices.Identity.Application.Hosting;

/// <summary>
/// 
/// </summary>
public class BaseUrlMiddleware
{
    private readonly RequestDelegate next;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="next"></param>
    public BaseUrlMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task Invoke(HttpContext context)
    {
        var urls = context.RequestServices.GetRequiredService<IServerUrls>();

        urls.BasePath = context.Request.PathBase.Value;

        await next(context);
    }
}