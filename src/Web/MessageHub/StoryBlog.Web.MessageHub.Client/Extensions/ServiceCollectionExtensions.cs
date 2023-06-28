using Microsoft.Extensions.DependencyInjection;
using StoryBlog.Web.Hub.Blazor.WebAssembly.Services;
using StoryBlog.Web.MessageHub.Configuration;

namespace StoryBlog.Web.MessageHub.Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessageHub(this IServiceCollection services, Action<MessageHubConfigurationBuilder> configure)
    {
        var builder = new MessageHubConfigurationBuilder();

        configure.Invoke(builder);

        var configuration = builder.Build();


        services.AddHostedService<ClientHostedService>();

        return services;
    }
}