using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StoryBlog.Web.Identity.Core;
using StoryBlog.Web.Microservices.Identity.Application.Configuration;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using System.Security.Claims;

namespace StoryBlog.Web.Microservices.Identity.Application.Extensions;

public static class ServiceCollectionExtensions
{
    internal static void AddDecorator<TService>(this IServiceCollection services)
    {
        var registration = services.LastOrDefault(x => x.ServiceType == typeof(TService));

        if (null == registration)
        {
            throw new InvalidOperationException("Service type: " + typeof(TService).Name + " not registered.");
        }

        if (services.Any(x => x.ServiceType == typeof(Decorator<TService>)))
        {
            throw new InvalidOperationException("Decorator already registered for type: " + typeof(TService).Name + ".");
        }

        services.Remove(registration);

        if (null != registration.ImplementationInstance)
        {
            var type = registration.ImplementationInstance.GetType();
            var innerType = typeof(Decorator<,>).MakeGenericType(typeof(TService), type);

            services.Add(new ServiceDescriptor(typeof(Decorator<TService>), innerType, ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(type, registration.ImplementationInstance));
        }
        else if (null != registration.ImplementationFactory)
        {
            services.Add(new ServiceDescriptor(
                typeof(Decorator<TService>),
                provider => Decorator<TService>.CreateDisposable((TService)registration.ImplementationFactory(provider)),
                registration.Lifetime)
            );
        }
        else
        {
            var type = registration.ImplementationType;
            var innerType = typeof(Decorator<,>).MakeGenericType(typeof(TService), registration.ImplementationType!);
            services.Add(new ServiceDescriptor(typeof(Decorator<TService>), innerType, ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(type!, type!, registration.Lifetime));
        }
    }

    internal static void AddTransientDecorator<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    {
        services.AddDecorator<TService>();
        services.AddTransient<TService, TImplementation>();
    }

    public static IServiceCollection AddStoryBlogIdentity(this IServiceCollection services, Action<IdentityServerOptions> setupAction)
    {
        services
            .AddOptions<StoryBlogIdentityServerOptions<StoryBlogIdentityOptions>>()
            .BindConfiguration(StoryBlogIdentityServerOptions.SectionName, configuration =>
            {
                configuration.BindNonPublicProperties = false;
            })
            .Validate(options =>
            {
                return true;
            })
            .ValidateOnStart();

        services
            .AddAuthentication(authentication =>
            {
                authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(bearer =>
            {
                var temp = "ac1ef3e44eb6494fa9a9d81ae6b5aae5"u8.ToArray();

                bearer.RequireHttpsMetadata = false;
                bearer.SaveToken = true;
                bearer.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(temp),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RoleClaimType = ClaimTypes.Role,
                    ClockSkew = TimeSpan.Zero
                };

                bearer.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception is SecurityTokenExpiredException)
                        {
                            return context.Response.WriteJsonAsync(
                                StatusCodes.Status401Unauthorized,
                                Result.Fail("The Token is expired.")
                            );
                        }

                        return context.Response.WriteJsonAsync(
                            StatusCodes.Status500InternalServerError,
                            Result.Fail("An unhandled error has occurred.")
                        );
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();

                        if (false == context.Response.HasStarted)
                        {
                            return context.Response.WriteJsonAsync(
                                StatusCodes.Status401Unauthorized,
                                Result.Fail("You are not Authorized.")
                            );
                        }

                        return Task.CompletedTask;
                    },
                    OnForbidden = context => context.Response.WriteJsonAsync(
                        StatusCodes.Status403Forbidden,
                        Result.Fail("You are not authorized to access this resource.")
                    )
                };
            });

        services.AddAuthorization(authorization =>
        {
            const string key = "Permission";
            var permissions = Permissions.GetRegisteredPermissions();

            foreach (var permission in permissions)
            {
                authorization.AddPolicy(permission, policy => policy.RequireClaim(key, permission));
            }
        });

        services
            /*.AddCookiePolicy(options =>
            {

                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            })*/
            .Configure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, options =>
            {
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

        services.Configure(setupAction);

        return services;
    }
}