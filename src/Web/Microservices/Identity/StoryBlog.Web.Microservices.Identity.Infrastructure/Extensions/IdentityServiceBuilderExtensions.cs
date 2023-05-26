using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using StoryBlog.Web.Microservices.Identity.Application.Contexts;
using StoryBlog.Web.Microservices.Identity.Application.DependencyInjection;
using StoryBlog.Web.Microservices.Identity.Application.DependencyInjection.Options;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Application.Services;
using StoryBlog.Web.Microservices.Identity.Application.Stores;
using StoryBlog.Web.Microservices.Identity.Infrastructure.Persistence;
using StoryBlog.Web.Microservices.Identity.Infrastructure.Stores;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Extensions;

public static class IdentityServiceBuilderExtensions
{
    /// <summary>
    /// Configures defaults on Identity Server for ASP.NET Core scenarios.
    /// </summary>
    /// <typeparam name="TUser">The <typeparamref name="TUser"/> type.</typeparam>
    /// <typeparam name="TContext">The <typeparamref name="TContext"/> type.</typeparam>
    /// <param name="builder">The <see cref="IIdentityServerBuilder"/>.</param>
    /// <param name="configure">The <see cref="Action{ApplicationsOptions}"/>
    /// to configure the <see cref="ApiAuthorizationOptions"/>.</param>
    /// <returns>The <see cref="IIdentityServerBuilder"/>.</returns>
    public static IIdentityServerBuilder AddApiAuthorization<TUser, TContext>(
        this IIdentityServerBuilder builder,
        Action<ApiAuthorizationOptions> configure)
        where TUser : class
        where TContext : DbContext//, IPersistedGrantDbContext
    {
        if (null == configure)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        builder
            .AddAspNetIdentity<TUser>()
            //.AddOperationalStore<TContext>()
            //.ConfigureReplacedServices()
            //.AddIdentityResources()
            //.AddApiResources()
            .AddClients()
            //.AddSigningCredentials()
            ;

        builder.Services.Configure(configure);

        return builder;
    }

    /// <summary>
    /// Adds a signing key store.
    /// </summary>
    /// <typeparam name="T">The type of the concrete store that is registered in DI.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <returns>The builder.</returns>
    public static IIdentityServerBuilder AddSigningKeyStore<T>(this IIdentityServerBuilder builder)
        where T : class, ISigningKeyStore
    {
        builder.Services.AddTransient<ISigningKeyStore, T>();

        return builder;
    }

    /// <summary>
    /// Adds a persisted grant store.
    /// </summary>
    /// <typeparam name="T">The type of the concrete grant store that is registered in DI.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <returns>The builder.</returns>
    public static IIdentityServerBuilder AddPersistedGrantStore<T>(this IIdentityServerBuilder builder)
        where T : class, IPersistedGrantStore
    {
        builder.Services.AddTransient<IPersistedGrantStore, T>();

        return builder;
    }

    /// <summary>
    /// Adds a device flow store.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="builder">The builder.</param>
    public static IIdentityServerBuilder AddDeviceFlowStore<T>(this IIdentityServerBuilder builder)
        where T : class, IDeviceFlowStore
    {
        builder.Services.AddTransient<IDeviceFlowStore, T>();

        return builder;
    }

    /// <summary>
    /// Add Configuration DbContext to the DI system.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="storeOptionsAction">The store options action.</param>
    /// <returns></returns>
    public static IServiceCollection AddConfigurationDbContext(
        this IServiceCollection services,
        Action<ConfigurationStoreOptions>? storeOptionsAction = null) =>
        services.AddConfigurationDbContext<StoryBlogIdentityDbContext>(storeOptionsAction);

    /// <summary>
    /// Add Configuration DbContext to the DI system.
    /// </summary>
    /// <typeparam name="TContext">The IConfigurationDbContext to use.</typeparam>
    /// <param name="services"></param>
    /// <param name="storeOptionsAction">The store options action.</param>
    /// <returns></returns>
    public static IServiceCollection AddConfigurationDbContext<TContext>(
        this IServiceCollection services,
        Action<ConfigurationStoreOptions>? storeOptionsAction = null)
        where TContext : DbContext, IConfigurationDbContext
    {
        var options = new ConfigurationStoreOptions();

        services.AddSingleton(options);
        storeOptionsAction?.Invoke(options);

        /*if (null != options.ResolveDbContextOptions)
        {
            if (options.EnablePooling)
            {
                if (options.PoolSize.HasValue)
                {
                    services.AddDbContextPool<TContext>(options.ResolveDbContextOptions, options.PoolSize.Value);
                }
                else
                {
                    services.AddDbContextPool<TContext>(options.ResolveDbContextOptions);
                }
            }
            else
            {
                services.AddDbContext<TContext>(options.ResolveDbContextOptions);
            }
        }
        else
        {
            if (options.EnablePooling)
            {
                if (options.PoolSize.HasValue)
                {
                    services.AddDbContextPool<TContext>(
                        dbCtxBuilder => options.ConfigureDbContext?.Invoke(dbCtxBuilder),
                        options.PoolSize.Value
                    );
                }
                else
                {
                    services.AddDbContextPool<TContext>(
                        dbCtxBuilder => options.ConfigureDbContext?.Invoke(dbCtxBuilder)
                    );
                }
            }
            else
            {
                services.AddDbContext<TContext>(dbCtxBuilder =>
                {
                    options.ConfigureDbContext?.Invoke(dbCtxBuilder);
                });
            }
        }

        services.AddScoped<IConfigurationDbContext, TContext>();*/

        return services;
    }

    /// <summary>
    /// Adds operational DbContext to the DI system.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="storeOptionsAction">The store options action.</param>
    /// <returns></returns>
    public static IServiceCollection AddOperationalDbContext(
        this IServiceCollection services,
        Action<OperationalStoreOptions>? storeOptionsAction = null) =>
        services.AddOperationalDbContext<StoryBlogIdentityDbContext>(storeOptionsAction);

    /// <summary>
    /// Adds operational DbContext to the DI system.
    /// </summary>
    /// <typeparam name="TContext">The IPersistedGrantDbContext to use.</typeparam>
    /// <param name="services"></param>
    /// <param name="storeOptionsAction">The store options action.</param>
    /// <returns></returns>
    public static IServiceCollection AddOperationalDbContext<TContext>(
        this IServiceCollection services,
        Action<OperationalStoreOptions>? storeOptionsAction = null)
        where TContext : DbContext, IPersistedGrantDbContext
    {
        var storeOptions = new OperationalStoreOptions();

        services.AddSingleton(storeOptions);
        storeOptionsAction?.Invoke(storeOptions);

        if (null != storeOptions.ResolveDbContextOptions)
        {
            if (storeOptions.EnablePooling)
            {
                if (storeOptions.PoolSize.HasValue)
                {
                    services.AddDbContextPool<TContext>(storeOptions.ResolveDbContextOptions,
                        storeOptions.PoolSize.Value);
                }
                else
                {
                    services.AddDbContextPool<TContext>(storeOptions.ResolveDbContextOptions);
                }
            }
            else
            {
                services.AddDbContext<TContext>(storeOptions.ResolveDbContextOptions);
            }
        }
        else
        {
            if (storeOptions.EnablePooling)
            {
                if (storeOptions.PoolSize.HasValue)
                {
                    services.AddDbContextPool<TContext>(
                        dbCtxBuilder => { storeOptions.ConfigureDbContext?.Invoke(dbCtxBuilder); },
                        storeOptions.PoolSize.Value);
                }
                else
                {
                    services.AddDbContextPool<TContext>(
                        dbCtxBuilder => { storeOptions.ConfigureDbContext?.Invoke(dbCtxBuilder); });
                }
            }
            else
            {
                services.AddDbContext<TContext>(dbCtxBuilder =>
                {
                    storeOptions.ConfigureDbContext?.Invoke(dbCtxBuilder);
                });
            }
        }

        services.AddScoped<IPersistedGrantDbContext, TContext>();
        services.AddTransient<TokenCleanupService>();

        return services;
    }

    /// <summary>
    /// Configures EF implementation of IClientStore, IResourceStore, and ICorsPolicyService with IdentityServer.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="storeOptionsAction">The store options action.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddConfigurationStore(
        this IIdentityServerBuilder builder,
        Action<ConfigurationStoreOptions>? storeOptionsAction = null) =>
        builder.AddConfigurationStore<StoryBlogIdentityDbContext>(storeOptionsAction);

    /// <summary>
    /// Configures EF implementation of IClientStore, IResourceStore, and ICorsPolicyService with IdentityServer.
    /// </summary>
    /// <typeparam name="TContext">The IConfigurationDbContext to use.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <param name="storeOptionsAction">The store options action.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddConfigurationStore<TContext>(
        this IIdentityServerBuilder builder,
        Action<ConfigurationStoreOptions>? storeOptionsAction = null)
        where TContext : DbContext, IConfigurationDbContext
    {
        builder.Services.AddConfigurationDbContext<TContext>(storeOptionsAction);

        builder.AddClientStore<ClientStore>();
        builder.AddResourceStore<ResourceStore>();
        builder.AddCorsPolicyService<CorsPolicyService>();
        //builder.AddIdentityProviderStore<IdentityProviderStore>();

        return builder;
    }

    /// <summary>
    /// Configures EF implementation of IPersistedGrantStore with IdentityServer.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="storeOptionsAction">The store options action.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddOperationalStore(
        this IIdentityServerBuilder builder,
        Action<OperationalStoreOptions>? storeOptionsAction = null)
    {
        return builder.AddOperationalStore<StoryBlogIdentityDbContext>(storeOptionsAction);
    }

    /// <summary>
    /// Configures EF implementation of IPersistedGrantStore with IdentityServer.
    /// </summary>
    /// <typeparam name="TContext">The IPersistedGrantDbContext to use.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <param name="storeOptionsAction">The store options action.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddOperationalStore<TContext>(
        this IIdentityServerBuilder builder,
        Action<OperationalStoreOptions>? storeOptionsAction = null)
        where TContext : DbContext, IPersistedGrantDbContext
    {
        builder.Services.AddOperationalDbContext<TContext>(storeOptionsAction);

        builder.AddSigningKeyStore<SigningKeyStore>();
        builder.AddPersistedGrantStore<PersistedGrantStore>();
        builder.AddDeviceFlowStore<DeviceFlowStore>();
        //builder.AddServerSideSessionStore<ServerSideSessionStore>();

        builder.Services.AddSingleton<IHostedService, TokenCleanupHostService>();

        return builder;
    }
}