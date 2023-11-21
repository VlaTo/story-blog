using StoryBlog.Web.Microservices.Identity.Application.Configuration;
using StoryBlog.Web.Microservices.Identity.Application;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Application.Services;
using StoryBlog.Web.Microservices.Identity.Domain.Entities;
using StoryBlog.Web.Microservices.Identity.Infrastructure.Extensions;
using StoryBlog.Web.Microservices.Identity.WebApi.Core;
using StoryBlog.Web.Microservices.Identity.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.dbconnection.json", optional: true, reloadOnChange: true);
builder.Services.AddMediatR(mediatr =>
{
    mediatr.RegisterServicesFromAssembly(typeof(IdentityServerConstants).Assembly);
});
builder.Services.AddAutoMapper(configuration =>
{
    configuration.AddApplicationMappingProfiles();
    //configuration.AddWebApiMappingProfiles();
});
builder.Services.AddScoped<IAuthenticationEventSink, LogAuthenticationEventSink>();
builder.Services.AddStoryBlogIdentity(options =>
{
    options.Cors.CorsPolicyName = "__DefaultCorsPolicy";

    //options.UserInteraction.LoginUrl = "http://localhost:50376/Authenticate/login";
    //options.UserInteraction.ErrorUrl = "http://localhost:5000/error";

    options.Events = new EventsOptions
    {
        RaiseErrorEvents = true,
        RaiseFailureEvents = true,
        RaiseInformationEvents = true,
        RaiseSuccessEvents = true
    };

    options.Authentication.CookieAuthenticationScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme;
    options.Authentication.CookieLifetime = TimeSpan.FromMinutes(10.0d);
    options.Authentication.CookieSlidingExpiration = true;
});
builder.Services.AddInfrastructure(builder.Configuration, "Database");
builder.Services
    .AddIdentityServer()
    .AddApiAuthorization<StoryBlogUser, StoryBlogRole>(options =>
    {
        options.Clients.AddSPA("288849a891664840975fa7992f247947", client =>
            client
                .WithScopes(
                    DefinedScopes.Blog.Api.Blogs,
                    DefinedScopes.Blog.Api.Comments,
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                )
        );
    })
    .AddJwtBearerClientAuthentication()
    .AddConfigurationStore()
    .AddOperationalStore();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();
builder.Services.AddCors(options => options
    .AddDefaultPolicy(policy => policy
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    )
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseExceptionHandler("/Home/Error");
}

app.UseIdentityServer();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

await app.RunAsync();
