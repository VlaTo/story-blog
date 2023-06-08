using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Infrastructure;
using StoryBlog.Web.Microservices.Identity.Application.Contexts;
using StoryBlog.Web.Microservices.Identity.Application.Models.Messages;
using StoryBlog.Web.Microservices.Identity.Application.Stores;
using StoryBlog.Web.Microservices.Identity.Domain.Entities;
using StoryBlog.Web.Microservices.Identity.Infrastructure.Persistence;
using StoryBlog.Web.Microservices.Identity.Infrastructure.Stores;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, string connectionStringKey)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork<StoryBlogIdentityDbContext>>();

        services.TryAddTransient<IUserConsentStore, UserConsentStore>();
        services.TryAddTransient<IRefreshTokenStore, DefaultRefreshTokenStore>();
        services.TryAddTransient<IMessageStore<LogoutNotificationContext>, ProtectedDataMessageStore<LogoutNotificationContext>>();
        services.TryAddTransient<IMessageStore<ErrorMessage>, ProtectedDataMessageStore<ErrorMessage>>();
        services.TryAddTransient<IMessageStore<LogoutMessage>, ProtectedDataMessageStore<LogoutMessage>>();
        services.TryAddTransient<IAuthorizationCodeStore, DefaultAuthorizationCodeStore>();
        services.TryAddTransient<IBackChannelAuthenticationRequestStore, DefaultBackChannelAuthenticationRequestStore>();
        services.TryAddTransient<IConsentMessageStore, ConsentMessageStore>();

        services
            .AddDbContext<StoryBlogIdentityDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString(connectionStringKey);

                options
                    .UseSqlServer(connectionString, context =>
                    {
                        var assemblyName = typeof(StoryBlogIdentityDbContext).Assembly.FullName;
                        context.MigrationsAssembly(assemblyName);
                    })
                    .EnableDetailedErrors(detailedErrorsEnabled: true);
            })
            .AddIdentity<StoryBlogUser, StoryBlogUserRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.SignIn.RequireConfirmedAccount = true;
            })
            .AddEntityFrameworkStores<StoryBlogIdentityDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}