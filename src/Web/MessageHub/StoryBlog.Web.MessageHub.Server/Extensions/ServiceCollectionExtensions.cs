using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.DependencyInjection;
using StoryBlog.Web.MessageHub.Configuration;
using StoryBlog.Web.MessageHub.Server.Middlewares;
using StoryBlog.Web.MessageHub.Server.Services;
using StoryBlog.Web.MessageHub.Services;

namespace StoryBlog.Web.MessageHub.Server.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessageHub(this IServiceCollection services, Action<MessageHubOptions> configuration)
    {
        services.AddConnections();
        services.AddWebSockets(sockets =>
        {
            sockets.KeepAliveInterval = TimeSpan.FromMinutes(15.0d);
        });

        services
            .AddOptions<MessageHubOptions>()
            .Configure(configuration)
            .PostConfigure(options =>
            {
                if (null == options.Serializer)
                {
                    options.Serializer = new JsonHubMessageSerializer();
                }
            })
            .Validate(options =>
            {

                return null != options.Serializer;
            })
            .ValidateOnStart();

        services
            .AddSingleton<MessageHubService>()
            .AddScoped<WebSocketMessageHubMiddleware>()
            .AddScoped<IMessageHub, WebSocketMessageHub>();

        return services;
    }
}