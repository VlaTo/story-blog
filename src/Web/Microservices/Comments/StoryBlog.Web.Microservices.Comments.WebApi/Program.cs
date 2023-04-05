using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using StoryBlog.Web.Identity.Extensions;
using StoryBlog.Web.Microservices.Comments.Application.Extensions;
using StoryBlog.Web.Microservices.Comments.Application.Handlers;
using StoryBlog.Web.Microservices.Comments.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.authentication.json", optional: true);

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssembly(typeof(HandlerBase).Assembly);
});
builder.Services.AddAutoMapper(configuration =>
{
    configuration.AddApplicationMappingProfiles();
    configuration.AddWebApiMappingProfiles();
});
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0, "alpha");
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod();
    });
});

builder.Services.AddStoryBlogAuthentication();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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