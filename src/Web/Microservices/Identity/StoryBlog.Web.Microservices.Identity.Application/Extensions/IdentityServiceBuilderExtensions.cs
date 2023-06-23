using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StoryBlog.Web.Microservices.Identity.Application.Configuration;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.DependencyInjection;
using StoryBlog.Web.Microservices.Identity.Application.DependencyInjection.Options;
using StoryBlog.Web.Microservices.Identity.Application.Endpoints;
using StoryBlog.Web.Microservices.Identity.Application.Hosting;
using StoryBlog.Web.Microservices.Identity.Application.ResponseHandling.Defaults;
using StoryBlog.Web.Microservices.Identity.Application.ResponseHandling.Generators;
using StoryBlog.Web.Microservices.Identity.Application.Services;
using StoryBlog.Web.Microservices.Identity.Application.Services.Defaults;
using StoryBlog.Web.Microservices.Identity.Application.Services.KeyManagement;
using StoryBlog.Web.Microservices.Identity.Application.Stores;
using StoryBlog.Web.Microservices.Identity.Application.Validation;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Defaults;
using DiscoveryEndpoint = StoryBlog.Web.Microservices.Identity.Application.Endpoints.DiscoveryEndpoint;
using Endpoint = StoryBlog.Web.Microservices.Identity.Application.Hosting.Endpoint;

namespace StoryBlog.Web.Microservices.Identity.Application.Extensions;

public static class IdentityServiceBuilderExtensions
{
    public static IIdentityServerBuilder AddIdentityServerCore(this IServiceCollection services)
    {
        return new IdentityServerBuilder(services);
    }

    public static IIdentityServerBuilder AddIdentityServer(this IServiceCollection services)
    {
        var builder = services.AddIdentityServerCore();

        builder
            .AddRequiredPlatformServices()
            .AddCookieAuthentication()
            .AddCoreServices()
            .AddKeyManagement()
            .AddDefaultEndpoints()
            .AddResponseGenerators()
            .AddDefaultSecretParsers()
            .AddDefaultSecretValidators()
            ;

        return builder;
    }

    /// <summary>
    /// Adds the default secret validators.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddDefaultSecretValidators(this IIdentityServerBuilder builder)
    {
        builder.Services.AddTransient<ISecretValidator, HashedSharedSecretValidator>();

        return builder;
    }

    /// <summary>
    /// Adds the default secret parsers.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddDefaultSecretParsers(this IIdentityServerBuilder builder)
    {
        builder.Services.AddTransient<ISecretParser, BasicAuthenticationSecretParser>();
        builder.Services.AddTransient<ISecretParser, PostBodySecretParser>();

        return builder;
    }

    /// <summary>
    /// Adds support for client authentication using JWT bearer assertions.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddJwtBearerClientAuthentication(this IIdentityServerBuilder builder)
    {
        builder.Services.TryAddTransient<IReplayCache, DefaultReplayCache>();
        builder.AddSecretParser<JwtBearerClientAssertionSecretParser>();
        builder.AddSecretValidator<PrivateKeyJwtSecretValidator>();

        return builder;
    }

    /// <summary>
    /// Adds the required platform services.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddRequiredPlatformServices(this IIdentityServerBuilder builder)
    {
        builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.TryAddTransient<IScopeParser, DefaultScopeParser>();
        builder.Services.TryAddTransient<IAuthorizeRequestValidator, AuthorizeRequestValidator>();
        builder.Services.TryAddTransient<IClientConfigurationValidator, DefaultClientConfigurationValidator>();
        builder.Services.TryAddTransient<IEventService, DefaultEventService>();
        builder.Services.TryAddTransient<IEventSink, DefaultEventSink>();
        builder.Services.TryAddTransient<ICustomAuthorizeRequestValidator, DefaultCustomAuthorizeRequestValidator>();
        builder.Services.TryAddTransient<IResourceValidator, DefaultResourceValidator>();
        builder.Services.AddOptions();
        builder.Services.AddSingleton(
            resolver => resolver.GetRequiredService<IOptions<IdentityServerOptions>>().Value
        );
        builder.Services.AddSingleton(
            resolver => resolver.GetRequiredService<IOptions<IdentityServerOptions>>().Value.DynamicProviders
        );

        //builder.Services.AddTransient(
        //    resolver => resolver.GetRequiredService<IOptions<IdentityServerOptions>>().Value.PersistentGrants
        //);

        builder.Services.AddHttpClient();

        return builder;
    }

    /// <summary>
    /// Adds key management services.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddKeyManagement(this IIdentityServerBuilder builder)
    {
        builder.Services.TryAddTransient<IAutomaticKeyManagerKeyStore, AutomaticKeyManagerKeyStore>();
        builder.Services.TryAddTransient<IKeyManager, KeyManager>();
        builder.Services.TryAddTransient<ISigningKeyProtector, DataProtectionKeyProtector>();
        builder.Services.TryAddTransient(
            provider => provider.GetRequiredService<IdentityServerOptions>().KeyManagement
        );
        builder.Services.TryAddSingleton<ISigningKeyStoreCache, InMemoryKeyStoreCache>();
        builder.Services.TryAddSingleton<ISigningKeyStore>(x =>
        {
            var options = x.GetRequiredService<IdentityServerOptions>();
            var logger = x.GetRequiredService<ILogger<FileSystemKeyStore>>();
            return new FileSystemKeyStore(options.KeyManagement.KeyPath, logger);
        });

        return builder;
    }

    /// <summary>
    /// Adds the default endpoints.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddDefaultEndpoints(this IIdentityServerBuilder builder)
    {
        builder.Services.AddTransient<IEndpointRouter, EndpointRouter>();

        builder.AddEndpoint<AuthorizeCallbackEndpoint>(Constants.EndpointNames.Authorize, Constants.ProtocolRoutePaths.AuthorizeCallback.EnsureLeadingSlash());
        builder.AddEndpoint<AuthorizeEndpoint>(Constants.EndpointNames.Authorize, Constants.ProtocolRoutePaths.Authorize.EnsureLeadingSlash());
        builder.AddEndpoint<BackchannelAuthenticationEndpoint>(Constants.EndpointNames.BackchannelAuthentication, Constants.ProtocolRoutePaths.BackchannelAuthentication.EnsureLeadingSlash());
        builder.AddEndpoint<CheckSessionEndpoint>(Constants.EndpointNames.CheckSession, Constants.ProtocolRoutePaths.CheckSession.EnsureLeadingSlash());
        //builder.AddEndpoint<DeviceAuthorizationEndpoint>(EndpointNames.DeviceAuthorization, ProtocolRoutePaths.DeviceAuthorization.EnsureLeadingSlash());
        builder.AddEndpoint<DiscoveryKeyEndpoint>(Constants.EndpointNames.Discovery, Constants.ProtocolRoutePaths.DiscoveryWebKeys.EnsureLeadingSlash());
        builder.AddEndpoint<DiscoveryEndpoint>(Constants.EndpointNames.Discovery, Constants.ProtocolRoutePaths.DiscoveryConfiguration.EnsureLeadingSlash());
        //builder.AddEndpoint<EndSessionCallbackEndpoint>(EndpointNames.EndSession, ProtocolRoutePaths.EndSessionCallback.EnsureLeadingSlash());
        //builder.AddEndpoint<EndSessionEndpoint>(EndpointNames.EndSession, ProtocolRoutePaths.EndSession.EnsureLeadingSlash());
        builder.AddEndpoint<IntrospectionEndpoint>(Constants.EndpointNames.Introspection, Constants.ProtocolRoutePaths.Introspection.EnsureLeadingSlash());
        //builder.AddEndpoint<TokenRevocationEndpoint>(EndpointNames.Revocation, ProtocolRoutePaths.Revocation.EnsureLeadingSlash());
        builder.AddEndpoint<TokenEndpoint>(Constants.EndpointNames.Token, Constants.ProtocolRoutePaths.Token.EnsureLeadingSlash());
        builder.AddEndpoint<UserInfoEndpoint>(Constants.EndpointNames.UserInfo, Constants.ProtocolRoutePaths.UserInfo.EnsureLeadingSlash());

        return builder;
    }

    /// <summary>
    /// Adds the response generators.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddResponseGenerators(this IIdentityServerBuilder builder)
    {
        builder.Services.TryAddTransient<ITokenResponseGenerator, DefaultTokenResponseGenerator>();
        builder.Services.TryAddTransient<IUserInfoResponseGenerator, DefaultUserInfoResponseGenerator>();
        //builder.Services.TryAddTransient<IIntrospectionResponseGenerator, IntrospectionResponseGenerator>();
        builder.Services.TryAddTransient<IAuthorizeInteractionResponseGenerator, DefaultAuthorizeInteractionResponseGenerator>();
        builder.Services.TryAddTransient<IAuthorizeResponseGenerator, DefaultAuthorizeResponseGenerator>();
        builder.Services.TryAddTransient<IDiscoveryResponseGenerator, DefaultDiscoveryResponseGenerator>();
        //builder.Services.TryAddTransient<ITokenRevocationResponseGenerator, TokenRevocationResponseGenerator>();
        //builder.Services.TryAddTransient<IDeviceAuthorizationResponseGenerator, DeviceAuthorizationResponseGenerator>();
        builder.Services.TryAddTransient<IBackchannelAuthenticationResponseGenerator, BackchannelAuthenticationResponseGenerator>();

        return builder;
    }

    /// <summary>
    /// Adds the secret parser.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="builder">The builder.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddSecretParser<T>(this IIdentityServerBuilder builder)
        where T : class, ISecretParser
    {
        builder.Services.AddTransient<ISecretParser, T>();

        return builder;
    }

    /// <summary>
    /// Adds the secret validator.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="builder">The builder.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddSecretValidator<T>(this IIdentityServerBuilder builder)
        where T : class, ISecretValidator
    {
        builder.Services.AddTransient<ISecretValidator, T>();

        return builder;
    }

    public static IIdentityServerBuilder AddCookieAuthentication(this IIdentityServerBuilder builder)
    {
        return builder
            .AddDefaultCookieHandlers()
            .AddCookieAuthenticationExtensions();
    }

    /// <summary>
    /// Adds the default cookie handlers and corresponding configuration
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddDefaultCookieHandlers(this IIdentityServerBuilder builder)
    {
        builder.Services
            .AddAuthentication(IdentityServerConstants.DefaultCookieAuthenticationScheme)
            .AddCookie(IdentityServerConstants.DefaultCookieAuthenticationScheme)
            .AddCookie(IdentityServerConstants.ExternalCookieAuthenticationScheme);

        builder.Services
            .AddSingleton<IConfigureOptions<CookieAuthenticationOptions>, ConfigureInternalCookieOptions>();

        return builder;
    }

    /// <summary>
    /// Adds the necessary decorators for cookie authentication required by IdentityServer
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddCookieAuthenticationExtensions(this IIdentityServerBuilder builder)
    {
        builder.Services.AddSingleton<IPostConfigureOptions<CookieAuthenticationOptions>, PostConfigureInternalCookieOptions>();
        builder.Services.AddTransientDecorator<IAuthenticationService, IdentityServerAuthenticationService>();
        builder.Services.AddTransientDecorator<IAuthenticationHandlerProvider, FederatedSignoutAuthenticationHandlerProvider>();

        return builder;
    }
    
    public static IIdentityServerBuilder AddCoreServices(this IIdentityServerBuilder builder)
    {
        builder.Services.AddTransient<IServerUrls, DefaultServerUrls>();
        builder.Services.AddTransient<IIssuerNameService, DefaultIssuerNameService>();
        builder.Services.AddTransient<ISecretsListParser, DefaultSecretsListParser>();
        builder.Services.AddTransient<ISecretsListValidator, DefaultSecretsListValidator>();
        builder.Services.AddTransient<ExtensionGrantValidator>();
        builder.Services.AddTransient<BearerTokenUsageValidator>();
        builder.Services.AddTransient<IJwtRequestValidator, JwtRequestValidator>();

        builder.Services.AddTransient<ReturnUrlParser>();
        //---builder.Services.AddTransient<IdentityServerTools>();

        builder.Services.AddTransient<IReturnUrlParser, OidcReturnUrlParser>();
        builder.Services.AddScoped<IUserSession, DefaultUserSession>();
        builder.Services.AddTransient(typeof(MessageCookie<>));

        builder.Services.AddCors();
        builder.Services.AddTransientDecorator<ICorsPolicyProvider, CorsPolicyProvider>();

        builder.Services.TryAddTransient<IConsentService, DefaultConsentService>();
        builder.Services.TryAddTransient<ICorsPolicyService, DefaultCorsPolicyService>();
        builder.Services.TryAddTransient<ICancellationTokenProvider, DefaultHttpContextCancellationTokenProvider>();
        //NOTE: moved to infrastructure builder.Services.TryAddTransient<IMessageStore<LogoutNotificationContext>, ProtectedDataMessageStore<LogoutNotificationContext>>();
        //NOTE: moved to infrastructure builder.Services.TryAddTransient<IMessageStore<ErrorMessage>, ProtectedDataMessageStore<ErrorMessage>>();
        //NOTE: moved to infrastructure builder.Services.TryAddTransient<IAuthorizationCodeStore, DefaultAuthorizationCodeStore>();
        builder.Services.TryAddTransient<IReferenceTokenStore, DefaultReferenceTokenStore>();
        //builder.Services.TryAddTransient<IConsentMessageStore, ConsentMessageStore>();
        //builder.Services.TryAddTransient<IRefreshTokenStore, DefaultRefreshTokenStore>();
        //builder.Services.TryAddTransient<IBackChannelAuthenticationRequestStore, DefaultBackChannelAuthenticationRequestStore>();
        //NOTE: moved to infrastructure builder.Services.TryAddTransient<IUserConsentStore, UserConsentStore>();
        builder.Services.TryAddTransient<IDeviceFlowCodeService, DefaultDeviceFlowCodeService>();
        builder.Services.TryAddTransient<IKeyMaterialService, DefaultKeyMaterialService>();
        builder.Services.TryAddTransient<ILogoutNotificationService, DefaultLogoutNotificationService>();
        builder.Services.TryAddTransient<ICustomTokenValidator, DefaultCustomTokenValidator>();
        builder.Services.TryAddTransient<IEndSessionRequestValidator, EndSessionRequestValidator>();
        builder.Services.TryAddTransient<IPersistentGrantSerializer, PersistentGrantSerializer>();
        builder.Services.TryAddTransient<IHandleGenerationService, DefaultHandleGenerationService>();
        builder.Services.TryAddTransient<ISessionCoordinationService, DefaultSessionCoordinationService>();
        builder.Services.TryAddTransient<ITokenService, DefaultTokenService>();
        builder.Services.TryAddTransient<ITokenCreationService, DefaultTokenCreationService>();
        builder.Services.TryAddTransient<IClaimsService, DefaultClaimsService>();
        builder.Services.TryAddTransient<IRefreshTokenService, DefaultRefreshTokenService>();
        builder.Services.TryAddTransient<IPersistedGrantService, DefaultPersistedGrantService>();
        
        builder.Services.TryAddTransient<IIdentityServerInteractionService, DefaultIdentityServerInteractionService>();

        builder.Services.AddTransient<IClientSecretValidator, ClientSecretValidator>();

        builder.AddJwtRequestUriHttpClient();
        builder.AddValidators();

        builder.Services.TryAddTransient(typeof(IConcurrencyLock<>), typeof(DefaultConcurrencyLock<>));

        builder.Services.TryAddTransient<IDeviceFlowThrottlingService, DistributedDeviceFlowThrottlingService>();
        builder.Services.TryAddTransient<IBackChannelAuthenticationThrottlingService, DistributedBackChannelAuthenticationThrottlingService>();
        builder.Services.TryAddTransient<IBackchannelAuthenticationUserNotificationService, NopBackchannelAuthenticationUserNotificationService>();

        builder.Services.AddDistributedMemoryCache();

        return builder;
    }

    /// <summary>
    /// Configures IdentityServer to use the ASP.NET Identity implementations 
    /// of IUserClaimsPrincipalFactory, IResourceOwnerPasswordValidator, and IProfileService.
    /// Also configures some of ASP.NET Identity's options for use with IdentityServer (such as claim types to use
    /// and authentication cookie settings).
    /// </summary>
    /// <typeparam name="TUser">The type of the user.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddAspNetIdentity<TUser, TRole>(this IIdentityServerBuilder builder)
        where TUser : IdentityUser
        where TRole : IdentityRole
    {
        builder.Services.AddTransientDecorator<IUserClaimsPrincipalFactory<TUser>, UserClaimsFactory<TUser>>();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.ClaimsIdentity.UserIdClaimType = JwtClaimTypes.Subject;
            options.ClaimsIdentity.UserNameClaimType = JwtClaimTypes.Name;
            options.ClaimsIdentity.RoleClaimType = JwtClaimTypes.Role;
            options.ClaimsIdentity.EmailClaimType = JwtClaimTypes.Email;
        });

        builder.Services.Configure<SecurityStampValidatorOptions>(opts =>
        {
            opts.OnRefreshingPrincipal = SecurityStampValidatorCallback.UpdatePrincipal;
        });

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.IsEssential = true;
            // we need to disable to allow iframe for authorize requests
            options.Cookie.SameSite = SameSiteMode.None;
        });

        builder.Services.ConfigureExternalCookie(options =>
        {
            options.Cookie.IsEssential = true;
            // https://github.com/IdentityServer/IdentityServer4/issues/2595
            options.Cookie.SameSite = SameSiteMode.None;
        });

        builder.Services.Configure<CookieAuthenticationOptions>(IdentityConstants.TwoFactorRememberMeScheme, options =>
        {
            options.Cookie.IsEssential = true;
        });

        builder.Services.Configure<CookieAuthenticationOptions>(IdentityConstants.TwoFactorUserIdScheme, options =>
        {
            options.Cookie.IsEssential = true;
        });

        builder.Services.AddAuthentication(options =>
        {
            if (options.DefaultAuthenticateScheme == null &&
                options.DefaultScheme == IdentityServerConstants.DefaultCookieAuthenticationScheme)
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
            }
        });

        //builder.Services.AddIdentity<TUser, TRole>();

        builder.AddResourceOwnerValidator<ResourceOwnerPasswordValidator<TUser>>();
        builder.AddProfileService<ProfileService<TUser>>();

        return builder;
    }

    // todo: check with later previews of ASP.NET Core if this is still required
    /// <summary>
    /// Adds configuration for the HttpClient used for JWT request_uri requests.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="configureClient">The configuration callback.</param>
    /// <returns></returns>
    public static IHttpClientBuilder AddJwtRequestUriHttpClient(this IIdentityServerBuilder builder, Action<HttpClient>? configureClient = null)
    {
        const string name = IdentityServerConstants.HttpClients.JwtRequestUriHttpClient;

        IHttpClientBuilder httpBuilder;

        if (null != configureClient)
        {
            httpBuilder = builder.Services.AddHttpClient(name, configureClient);
        }
        else
        {
            httpBuilder = builder.Services.AddHttpClient(name).ConfigureHttpClient(client =>
                client.Timeout = TimeSpan.FromSeconds(IdentityServerConstants.HttpClients.DefaultTimeoutSeconds)
            );
        }

        builder.Services.AddTransient<IJwtRequestUriHttpClient, DefaultJwtRequestUriHttpClient>(s =>
        {
            var httpClientFactory = s.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient(name);
            var loggerFactory = s.GetRequiredService<ILoggerFactory>();
            var options = s.GetRequiredService<IdentityServerOptions>();

            return new DefaultJwtRequestUriHttpClient(httpClient, options, new NoneCancellationTokenProvider(), loggerFactory);
        });

        return httpBuilder;
    }

    /// <summary>
    /// Adds the resource owner validator.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="builder">The builder.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddResourceOwnerValidator<T>(this IIdentityServerBuilder builder)
        where T : class, IResourceOwnerPasswordValidator
    {
        builder.Services.AddTransient<IResourceOwnerPasswordValidator, T>();

        return builder;
    }

    /// <summary>
    /// Adds the backchannel login user validator.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="builder">The builder.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddBackchannelAuthenticationUserValidator<T>(this IIdentityServerBuilder builder)
        where T : class, IBackchannelAuthenticationUserValidator
    {
        builder.Services.AddTransient<IBackchannelAuthenticationUserValidator, T>();

        return builder;
    }

    /// <summary>
    /// Adds the profile service.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="builder">The builder.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddProfileService<T>(this IIdentityServerBuilder builder)
        where T : class, IProfileService
    {
        builder.Services.AddTransient<IProfileService, T>();

        return builder;
    }

    public static IIdentityServerBuilder AddValidators(this IIdentityServerBuilder builder)
    {
        builder.Services.TryAddTransient<ITokenValidator, TokenValidator>();
        builder.Services.TryAddTransient<ICustomTokenRequestValidator, DefaultCustomTokenRequestValidator>();
        builder.Services.TryAddTransient<ITokenRequestValidator, TokenRequestValidator>();
        builder.Services.TryAddTransient<IDeviceCodeValidator, DeviceCodeValidator>();
        builder.Services.TryAddTransient<IUserInfoRequestValidator, UserInfoRequestValidator>();
        builder.Services.TryAddTransient<IBackchannelAuthenticationRequestValidator, BackchannelAuthenticationRequestValidator>();
        builder.Services.TryAddTransient<IBackchannelAuthenticationUserValidator, NopBackchannelAuthenticationUserValidator>();
        builder.Services.TryAddTransient<IBackChannelAuthenticationRequestIdValidator, BackChannelAuthenticationRequestIdValidator>();

        return builder;
    }

    /// <summary>
    /// Adds a client store.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="builder">The builder.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddClientStore<T>(this IIdentityServerBuilder builder)
        where T : class, IClientStore
    {
        builder.Services.TryAddTransient(typeof(T));
        builder.Services.AddTransient<IClientStore, ValidatingClientStore<T>>();

        return builder;
    }

    /// <summary>
    /// Adds the in memory clients.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="clients">The clients.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddInMemoryClients(this IIdentityServerBuilder builder, IEnumerable<Storage.Client> clients)
    {
        builder.Services.AddSingleton(clients);

        builder.AddClientStore<InMemoryClientStore>();

        var existingCors = builder.Services.LastOrDefault(x => x.ServiceType == typeof(ICorsPolicyService));
        if (null != existingCors &&
            typeof(DefaultCorsPolicyService) == existingCors.ImplementationType &&
            existingCors.Lifetime == ServiceLifetime.Transient)
        {
            // if our default is registered, then overwrite with the InMemoryCorsPolicyService
            // otherwise don't overwrite with the InMemoryCorsPolicyService, which uses the custom one registered by the host
            builder.Services.AddTransient<ICorsPolicyService, InMemoryCorsPolicyService>();
        }

        return builder;
    }

    /// <summary>
    /// Adds a resource store.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="builder">The builder.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddResourceStore<T>(this IIdentityServerBuilder builder)
        where T : class, IResourceStore
    {
        builder.Services.AddTransient<IResourceStore, T>();

        return builder;
    }

    /// <summary>
    /// Adds clients from the default configuration to the server using the key
    /// IdentityServer:Clients
    /// </summary>
    /// <param name="builder">The <see cref="IIdentityServerBuilder"/>.</param>
    /// <returns>The <see cref="IIdentityServerBuilder"/>.</returns>
    public static IIdentityServerBuilder AddClients(this IIdentityServerBuilder builder)
        => builder.AddClients(configuration: null);

    /// <summary>
    /// Adds clients from the given <paramref name="configuration"/> instance.
    /// </summary>
    /// <param name="builder">The <see cref="IIdentityServerBuilder"/>.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> instance containing the client definitions.</param>
    /// <returns>The <see cref="IIdentityServerBuilder"/>.</returns>
    public static IIdentityServerBuilder AddClients(this IIdentityServerBuilder builder, IConfiguration? configuration)
    {
        builder.ConfigureReplacedServices();
        builder.AddInMemoryClients(Enumerable.Empty<Storage.Client>());

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IPostConfigureOptions<ApiAuthorizationOptions>, ConfigureClientScopes>()
        );

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IConfigureOptions<ApiAuthorizationOptions>, ConfigureClients>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<ConfigureClients>>();
                var effectiveConfig = configuration ?? sp.GetRequiredService<IConfiguration>().GetSection("IdentityServer:Clients");
                return new ConfigureClients(effectiveConfig, logger);
            }));

        // We take over the setup for the clients as Identity Server registers the enumerable as a singleton and that prevents normal composition.
        builder.Services.AddSingleton<IEnumerable<Storage.Client>>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<ApiAuthorizationOptions>>();
            return options.Value.Clients;
        });

        return builder;
    }

    /// <summary>
    /// Adds a CORS policy service.
    /// </summary>
    /// <typeparam name="T">The type of the concrete scope store class that is registered in DI.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddCorsPolicyService<T>(this IIdentityServerBuilder builder)
        where T : class, ICorsPolicyService
    {
        builder.Services.AddTransient<ICorsPolicyService, T>();

        return builder;
    }

    /// <summary>
    /// Adds the endpoint.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="builder">The builder.</param>
    /// <param name="name">The name.</param>
    /// <param name="path">The path.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddEndpoint<T>(this IIdentityServerBuilder builder, string name, PathString path)
        where T : class, IEndpointHandler
    {
        builder.Services.AddTransient<T>();
        builder.Services.AddSingleton(new Endpoint(name, path, typeof(T)));

        return builder;
    }

    internal static IIdentityServerBuilder ConfigureReplacedServices(this IIdentityServerBuilder builder)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<IdentityServerOptions>, AspNetConventionsConfigureOptions>());
        builder.Services.TryAddSingleton<IAbsoluteUrlFactory, AbsoluteUrlFactory>();
        builder.Services.AddSingleton<IRedirectUriValidator, RelativeRedirectUriValidator>();
        builder.Services.AddSingleton<IClientRequestParametersProvider, DefaultClientRequestParametersProvider>();

        ReplaceEndSessionEndpoint(builder);

        return builder;
    }

    // Adding OpenIdConnectHandler
    // https://github.com/dotnet/aspnetcore/blob/c85baf8db0c72ae8e68643029d514b2e737c9fae/src/Security/Authentication/OpenIdConnect/src/OpenIdConnectHandler.cs

    private static void ReplaceEndSessionEndpoint(IIdentityServerBuilder builder)
    {
        const string name = "Endsession";
        const string path = "/connect/endsession";
        var comparer = StringComparer.OrdinalIgnoreCase;

        // We don't have a better way to replace the end session endpoint as far as we know other than looking the descriptor up
        // on the container and replacing the instance. This is due to the fact that we chain on AddIdentityServer which configures the
        // list of endpoints by default.
        var endSessionEndpointDescriptor = builder.Services.SingleOrDefault(descriptor =>
        {
            if (descriptor.ImplementationInstance is Endpoint endpoint)
            {
                return comparer.Equals(endpoint.Name, name) && comparer.Equals(endpoint.Path, path);
            }

            return false;
        });

        if (null != endSessionEndpointDescriptor && builder.Services.Remove(endSessionEndpointDescriptor))
        {
            ;
        }

        builder.AddEndpoint<AutoRedirectEndSessionEndpoint>(name, path);
    }
}