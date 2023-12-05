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
        services.AddSingleton<IBlogPostProcessingQueueProvider, BlogPostProcessingQueueProvider>();
        services.AddTransient<IBlogPostProcessingManager, BlogPostProcessingManager>();
        services.AddHostedService<BlogPostProcessingBackgroundService>();

        return services;
    }
}