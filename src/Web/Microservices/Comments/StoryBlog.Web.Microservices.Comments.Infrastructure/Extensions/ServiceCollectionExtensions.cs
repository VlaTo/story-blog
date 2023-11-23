using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Infrastructure;
using StoryBlog.Web.Microservices.Comments.Application.MessageBus.Handlers;
using StoryBlog.Web.Microservices.Comments.Infrastructure.Persistence;

namespace StoryBlog.Web.Microservices.Comments.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IAsyncUnitOfWork, AsyncUnitOfWork<CommentsDbContext>>();

        services.AddScoped<NewPostCreatedEventHandler>();
        services.AddScoped<PostPublishedEventHandler>();
        services.AddScoped<PostRemovedEventHandler>();

        return services;
    }

    public static IServiceCollection AddInfrastructureDbContext(this IServiceCollection services, IConfiguration configuration, string connectionStringKey)
    {
        services.AddDbContext<CommentsDbContext>(
            options =>
            {
                var connectionString = configuration.GetConnectionString(connectionStringKey);
                options
                    .UseNpgsql(connectionString, context =>
                    {
                        var assemblyName = typeof(CommentsDbContext).Assembly.FullName;
                        context.MigrationsAssembly(assemblyName);
                    })
                    .EnableDetailedErrors(detailedErrorsEnabled: true);
            });

        return services;
    }

}