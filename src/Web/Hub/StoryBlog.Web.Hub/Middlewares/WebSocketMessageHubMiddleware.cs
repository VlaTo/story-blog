using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Hub.Services;

namespace StoryBlog.Web.Hub.Middlewares;

internal sealed class WebSocketMessageHubMiddleware : IMiddleware
{
    private readonly ILogger<WebSocketMessageHubMiddleware> logger;

    public WebSocketMessageHubMiddleware(ILogger<WebSocketMessageHubMiddleware> logger)
    {
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var mypath = new PathString("/notification");

        if (context.WebSockets.IsWebSocketRequest)
        {
            if (mypath.Equals(context.Request.Path))
            {
                var socketManager = context.RequestServices.GetRequiredService<MessageHubService>();

                try
                {
                    var webSocket = await context.WebSockets.AcceptWebSocketAsync();

                    using (var handler = await socketManager.CreateWebSocketHandlerAsync(webSocket))
                    {
                        await handler.HandleAsync(context.RequestAborted);
                    }

                    return;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
            }
        }

        await next.Invoke(context);
    }
}