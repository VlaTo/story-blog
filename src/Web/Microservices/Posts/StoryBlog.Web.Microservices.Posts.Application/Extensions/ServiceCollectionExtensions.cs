using Microsoft.Extensions.DependencyInjection;
using StoryBlog.Web.Microservices.Posts.Application.Services;

namespace StoryBlog.Web.Microservices.Posts.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IWordTransliterator, RussianWordTransliterator>();

        return services;
    }
}