using Microsoft.Extensions.DependencyInjection;
using StoryBlog.Web.Microservices.Communication.Application.Handlers.NewPostCreated;

namespace StoryBlog.Web.Microservices.Communication.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<NewPostCreatedHandler>();

        return services;
    }
}