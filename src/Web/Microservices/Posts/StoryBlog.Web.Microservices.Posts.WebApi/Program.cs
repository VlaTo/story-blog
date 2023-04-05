
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using StoryBlog.Web.Identity.Extensions;
using StoryBlog.Web.Microservices.Posts.Application.Extensions;
using StoryBlog.Web.Microservices.Posts.Application.Handlers;
using StoryBlog.Web.Microservices.Posts.Infrastructure.Extensions;
using StoryBlog.Web.Microservices.Posts.WebApi.Configuration;
using StoryBlog.Web.Microservices.Posts.WebApi.Core;
using StoryBlog.Web.Microservices.Posts.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.authentication.json", optional: true);

builder.Services.AddInfrastructureServices();
builder.Services.AddInfrastructureDbContext(builder.Configuration, "PostsDb");
builder.Services.AddScoped<IPostLocationProvider, PostLocationProvider>();
builder.Services.AddApplicationServices();
builder.Services
    .AddOptions<PostLocationProviderOptions>()
    .BindConfiguration(PostLocationProviderOptions.SectionName);
builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssembly(typeof(HandlerBase).Assembly);
});
builder.Services.AddAutoMapper(configuration =>
{
    configuration.AddApplicationMappingProfiles();
    configuration.AddWebApiMappingProfiles();
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
builder.Services.AddStoryBlogAuthentication();
builder.Services.AddControllers();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0, "alpha");
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v 'VVVV";
    options.SubstituteApiVersionInUrl = true;
    options.SubstitutionFormat = "VVVV";
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant()
            );
        }
    });
}

app
    .UseHttpsRedirection()
    .UseApiVersioning()
    .UseCors()
    .UseAuthentication()
    .UseAuthorization();
app
    .MapControllers()
    .WithOpenApi();

await app.RunAsync();