using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.DependencyInjection;
using StoryBlog.Web.Hub.Common.Configuration;
using StoryBlog.Web.Hub.Services;
using StoryBlog.Web.Hub.Services.Hosted;

namespace StoryBlog.Web.Hub.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessageHub(this IServiceCollection services, Action<MessageHubOptions> configuration)
    {
        //var builder = new MessageHubOptionBuilder();

        //configuration.Invoke(builder);

        //var options = builder.Build();

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
                    options.Serializer = new JsonMessageSerializer();
                }
            })
            .Validate(options =>
            {

                return null != options.Serializer;
            })
            .ValidateOnStart();

        services.AddSingleton<MessageHubService>();
        services.AddScoped<IMessageHub, WebSocketMessageHub>();

        //services.AddHostedService<MessageHubHostedService>();

        return services;
    }
}