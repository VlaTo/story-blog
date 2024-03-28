using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
            .PostConfigure((PostsCreateOptions options, ILogger<PostsCreateOptions> logger) =>
            {
                if (options.ApprovePostWhenCreated)
                {
                    logger.LogWarning("New post will be approved when created");
                }
            });

        services.AddTransient<IWordTransliterator, RussianWordTransliterator>();
        services.AddSingleton<BlogPostProcessingManager>();
        services.AddSingleton<IBriefPostsPagingInfoProvider, BriefPostsPagingInfoProvider>();
        services.AddTransient<IBlogPostProcessingManager>(
            serviceProvider => serviceProvider.GetRequiredService<BlogPostProcessingManager>()
        );
        services.AddSingleton<IBlogPostProcessingQueue>(
            serviceProvider => serviceProvider.GetRequiredService<BlogPostProcessingManager>()
        );
        services.AddHostedService<BlogPostProcessingBackgroundService>();

        return services;
    }
}