﻿using Microsoft.Extensions.DependencyInjection;

namespace StoryBlog.Web.Microservices.Comments.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //services.AddTransient<IWordTransliterator, RussianWordTransliterator>();

        return services;
    }
}