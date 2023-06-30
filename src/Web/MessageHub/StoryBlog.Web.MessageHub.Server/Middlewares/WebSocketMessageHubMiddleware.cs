using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StoryBlog.Web.MessageHub.Server.Helpers;
using StoryBlog.Web.MessageHub.Server.Services;
using MessageHubOptions = StoryBlog.Web.MessageHub.Server.Configuration.MessageHubOptions;

namespace StoryBlog.Web.MessageHub.Server.Middlewares;

internal sealed class WebSocketMessageHubMiddleware : IMiddleware
{
    private readonly MessageHubOptions options;
    private readonly ILogger<WebSocketMessageHubMiddleware> logger;

    public WebSocketMessageHubMiddleware(
        IOptions<MessageHubOptions> options,
        ILogger<WebSocketMessageHubMiddleware> logger)
    {
        this.options = options.Value;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Path.Equals(options.Path))
        {
            var isMethod = StringHelper.Equals(context.Request.Method, HttpMethods.Get, HttpMethods.Connect);
            var isUpgrade = context.Request.Headers.Connection.Any(x => String.Equals(x, "Upgrade"));
            var isWebSocket = context.Request.Headers.Upgrade.Any(x => String.Equals(x, "websocket"));

            //if (context.WebSockets.IsWebSocketRequest)
            if (isMethod && isUpgrade && isWebSocket)
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