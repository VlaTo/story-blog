using Microsoft.Extensions.DependencyInjection;
using StoryBlog.Web.Hub.Blazor.WebAssembly.Services;
using StoryBlog.Web.Hub.Common.Configuration;

namespace StoryBlog.Web.Hub.Blazor.WebAssembly.Extensions;

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