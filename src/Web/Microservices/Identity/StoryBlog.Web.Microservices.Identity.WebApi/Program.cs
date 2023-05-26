using StoryBlog.Web.Microservices.Identity.Application.Configuration;
using StoryBlog.Web.Microservices.Identity.Application;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Domain.Entities;
using StoryBlog.Web.Microservices.Identity.Infrastructure.Extensions;
using StoryBlog.Web.Microservices.Identity.Infrastructure.Persistence;
using StoryBlog.Web.Microservices.Identity.WebApi.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddStoryBlogIdentity(options =>
{
    //options.Cors.CorsPolicyName = Constants.ClientPolicy;
    options.UserInteraction.LoginUrl = "http://localhost:5276/Authenticate/login";
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
    .AddApiAuthorization<StoryBlogUser, StoryBlogIdentityDbContext>(options =>
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

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseIdentityServer();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
