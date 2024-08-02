using Microsoft.Extensions.DependencyInjection;
using StoryBlog.Web.Identity.Client.Providers;

namespace StoryBlog.Web.Identity.Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHttpClientAuthorization(this IServiceCollection services)
    {
        services.AddMemoryCache();

        services.AddTransient<IAuthorizationTokenProvider, HttpClientAuthorizationTokenProvider>();

        return services;
    }
}