using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.DependencyInjection;
using StoryBlog.Web.MessageHub.Server.Middlewares;
using StoryBlog.Web.MessageHub.Server.Services;
using MessageHubOptions = StoryBlog.Web.MessageHub.Server.Configuration.MessageHubOptions;

namespace StoryBlog.Web.MessageHub.Server.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessageHub(this IServiceCollection services, Action<MessageHubOptions> setupAction)
    {
        services.AddConnections();
        services.AddWebSockets(sockets =>
        {
            sockets.KeepAliveInterval = TimeSpan.FromMinutes(15.0d);
        });

        services.AddOptions();
        services.Configure(setupAction);

        /*services
            .AddOptions<MessageHubOptions>()
            .Configure(setupAction)
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
            .ValidateOnStart();*/


        services
            .AddSingleton<MessageHubService>()
            .AddScoped<WebSocketMessageHubMiddleware>()
            .AddScoped<IMessageHub, WebSocketMessageHub>();

        return services;
    }
}