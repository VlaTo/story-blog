using StoryBlog.Web.Microservices.Communication.WebApi.MessageBus.Consumers;

namespace StoryBlog.Web.Microservices.Communication.WebApi.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddScoped<NewPostCreatedMessageConsumer>();

        return services;
    }
}