using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Identity.Configuration;
using System.Security.Claims;
using System.Text;
using System.Net.Mime;
using System.Text.Json;

namespace StoryBlog.Web.Identity.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStoryBlogAuthentication(this IServiceCollection services)
    {
        services
            .AddOptions<StoryBlogAuthenticationOptions>()
            .BindConfiguration(StoryBlogAuthenticationOptions.SectionName, configure =>
            {
                configure.BindNonPublicProperties = false;
            })
            .PostConfigure(options =>
            {
                ;
            })
            .Validate(options =>
            {
                return true;
            });

        services
            .AddAuthentication(authentication =>
            {
                authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(bearer =>
            {
                JwtBearerAuthenticationOptions jwt;

                using (var serviceProvider = services.BuildServiceProvider())
                {
                    var storyBlogAuthenticationOptions = serviceProvider.GetRequiredService<IOptions<StoryBlogAuthenticationOptions>>();
                    jwt = storyBlogAuthenticationOptions.Value.JwtBearer;
                }

                var securityKey = Encoding.UTF8.GetBytes(jwt.IssuerSigningKey);

                bearer.RequireHttpsMetadata = jwt.RequireHttpsMetadata;
                bearer.SaveToken = true;
                bearer.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtRegisteredClaimNames.Name,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(securityKey),
                    ValidateIssuer = jwt.ValidateIssuer.GetValueOrDefault(false),
                    ValidateAudience = jwt.ValidateAudience.GetValueOrDefault(false == String.IsNullOrEmpty(jwt.Audience)),
                    ValidateActor = jwt.ValidateActor.GetValueOrDefault(false),
                    ValidateLifetime = jwt.ValidateLifetime.GetValueOrDefault(false),
                    RoleClaimType = ClaimTypes.Role,
                    ClockSkew = TimeSpan.Zero
                };
                bearer.Audience = jwt.Audience;
                bearer.Authority = jwt.Authority;
                bearer.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception is SecurityTokenExpiredException)
                        {
                            return context.Response.WriteJsonAsync(
                                StatusCodes.Status401Unauthorized,
                                Result.Fail(new Exception("Token has expired"))
                            );
                        }

                        //context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        //context.Response.ContentType = MediaTypeNames.Application.Json;

                        Debug.WriteLine(context.Exception.Message);

                        //var bytes = Encoding.UTF8.GetBytes(context.Exception.Message);
                        //return context.Response.BodyWriter.WriteAsync(bytes).AsTask();
                        //return context.Response.Body.WriteAsync(bytes).AsTask();

                        /*return context.Response.WriteJsonAsync(
                            StatusCodes.Status500InternalServerError,
                            context.Exception.Message
                        );*/

                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();

                        if (false == context.Response.HasStarted)
                        {
                            return context.Response.WriteJsonAsync(
                                StatusCodes.Status401Unauthorized,
                                Result.Fail(new Exception("You are not authorized"))
                            );
                        }

                        return Task.CompletedTask;
                    },
                    OnForbidden = context =>
                    {
                        return context.Response.WriteJsonAsync(
                            StatusCodes.Status403Forbidden,
                            Result.Fail(new Exception("You are not authorized to access this resource."))
                        );
                    }
                };
            });

        services
            .AddAuthorization(authorization =>
            {
                /*const string permission = "Permission";
                var permissions = Permissions.GetRegisteredPermissions();

                for (var index = 0; index < permissions.Count; index++)
                {
                    var name = permissions[index];
                    authorization.AddPolicy(
                        name,
                        policy => policy.RequireClaim(permission, name)
                    );
                }*/
            });

        return services;
    }
}