using StoryBlog.Web.Microservices.Communication.Application.Services;
using StoryBlog.Web.Microservices.Communication.WebApi.MessageBus.Consumers;
using StoryBlog.Web.Microservices.Communication.WebApi.MessageHub.Notification;

namespace StoryBlog.Web.Microservices.Communication.WebApi.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddScoped<NewPostCreatedEventConsumer>();
        services.AddScoped<NewCommentCreatedEventConsumer>();

        services.AddScoped<IMessageHubNotification, MessageHubNotification>();

        return services;
    }
}