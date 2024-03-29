﻿using StoryBlog.Web.Microservices.Posts.Application.Services;
using StoryBlog.Web.Microservices.Posts.WebApi.Configuration;
using StoryBlog.Web.Microservices.Posts.WebApi.MessageBus.Consumers;
using StoryBlog.Web.Microservices.Posts.WebApi.MessageBus.Notification;

namespace StoryBlog.Web.Microservices.Posts.WebApi.Extensions;

internal static class ServicesExtensions
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services)
    {
        services
            .AddOptions<MessageBusOptions>()
            .BindConfiguration("MessageBus");

        services.AddScoped<IMessageBusNotification, SlimMessageBusNotification>();

        services.AddScoped<NewCommentCreatedEventConsumer>();

        return services;
    }
}