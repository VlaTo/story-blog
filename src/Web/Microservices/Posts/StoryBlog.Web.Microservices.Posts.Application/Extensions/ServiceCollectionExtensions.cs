using Microsoft.Extensions.DependencyInjection;
using StoryBlog.Web.Microservices.Posts.Application.Configuration;
using StoryBlog.Web.Microservices.Posts.Application.Services;

namespace StoryBlog.Web.Microservices.Posts.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services
            .AddOptions<PostsCreateOptions>()
            .BindConfiguration("PostsCreate")
            .PostConfigure(options =>
            {
                if (String.IsNullOrEmpty(options.HubChannelName))
                {
                    ;
                }
            });

        services.AddTransient<IWordTransliterator, RussianWordTransliterator>();
        services.AddSingleton<BackgroundWorkManager>();
        services.AddTransient<IBackgroundWorkManager>(
            serviceProvider => serviceProvider.GetRequiredService<BackgroundWorkManager>()
        );

        services.AddHostedService<SampleBackgroundService>();

        return services;
    }
}