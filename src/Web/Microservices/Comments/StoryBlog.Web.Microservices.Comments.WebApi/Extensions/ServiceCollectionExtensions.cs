﻿using StoryBlog.Web.Microservices.Comments.Application.Services;
using StoryBlog.Web.Microservices.Comments.WebApi.ApiClients;
using StoryBlog.Web.Microservices.Comments.WebApi.Configuration;
using StoryBlog.Web.Microservices.Comments.WebApi.Core;
using StoryBlog.Web.Microservices.Comments.WebApi.MessageBus.Consumers;
using StoryBlog.Web.Microservices.Comments.WebApi.MessageBus.Notification;

namespace StoryBlog.Web.Microservices.Comments.WebApi.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services)
    {
        services.AddScoped<IMessageBusNotification, SlimMessageBusNotification>();
        
        services.AddScoped<ILocationProvider, AspNetCoreLocationProvider>();

        services.AddScoped<NewPostCreatedEventConsumer>();
        
        services
            .AddOptions<CommentLocationProviderOptions>()
            .BindConfiguration(CommentLocationProviderOptions.SectionName);

        services.AddTransient<ApiAuthorizationMessageHandler>();

        return services;
    }
}