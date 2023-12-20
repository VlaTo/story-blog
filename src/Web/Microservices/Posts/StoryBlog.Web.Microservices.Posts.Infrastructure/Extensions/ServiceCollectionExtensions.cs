using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Infrastructure;
using StoryBlog.Web.Microservices.Posts.Application.MessageBus.Handlers;
using StoryBlog.Web.Microservices.Posts.Infrastructure.Persistence;

namespace StoryBlog.Web.Microservices.Posts.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IAsyncUnitOfWork, AsyncUnitOfWork<PostsDbContext>>();
        
        services.AddScoped<NewCommentCreatedEventHandler>();

        return services;
    }

    public static IServiceCollection AddInfrastructureDbContext(this IServiceCollection services, IConfiguration configuration, string connectionStringKey)
    {
        services.AddDbContext<PostsDbContext>(
            options =>
            {
                var connectionString = configuration.GetConnectionString(connectionStringKey);
                options
                    .EnableSensitiveDataLogging(sensitiveDataLoggingEnabled: true)
                    .UseNpgsql(connectionString, context =>
                    {
                        var assemblyName = typeof(PostsDbContext).Assembly.FullName;
                        context.MigrationsAssembly(assemblyName);
                    })
                    .EnableDetailedErrors(detailedErrorsEnabled: true);
            });

        return services;
    }
}