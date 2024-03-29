using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.IdentityModel.Logging;
using SlimMessageBus.Host;
using SlimMessageBus.Host.RabbitMQ;
using SlimMessageBus.Host.Serialization.SystemTextJson;
using StoryBlog.Web.Identity.Extensions;
using StoryBlog.Web.Microservices.Comments.Events;
using StoryBlog.Web.Microservices.Posts.Application.Contexts;
using StoryBlog.Web.Microservices.Posts.Application.Extensions;
using StoryBlog.Web.Microservices.Posts.Events;
using StoryBlog.Web.Microservices.Posts.Infrastructure.Extensions;
using StoryBlog.Web.Microservices.Posts.WebApi.Configuration;
using StoryBlog.Web.Microservices.Posts.WebApi.Core;
using StoryBlog.Web.Microservices.Posts.WebApi.Extensions;
using StoryBlog.Web.Microservices.Posts.WebApi.MessageBus.Consumers;
using System.Diagnostics.Tracing;
using System.Net.Mime;

var builder = WebApplication.CreateBuilder(args);

IdentityModelEventSource.Logger.LogLevel = EventLevel.Verbose;
IdentityModelEventSource.ShowPII = true;

builder.Configuration.AddJsonFile("appsettings.authentication.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile("appsettings.dbconnection.json", optional: true, reloadOnChange: true);

builder.Services.AddInfrastructureServices();
builder.Services.AddInfrastructureDbContext(builder.Configuration, "Database");
builder.Services.AddScoped<ILocationProvider, AspNetCoreLocationProvider>();
builder.Services.AddApplicationServices();
builder.Services.AddWebApiServices();
builder.Services
    .AddOptions<PostLocationProviderOptions>()
    .BindConfiguration(PostLocationProviderOptions.SectionName);
builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssembly(typeof(IPostsDbContext).Assembly);
});
builder.Services.AddAutoMapper(configuration =>
{
    configuration.AddApplicationMappingProfiles();
    configuration.AddWebApiMappingProfiles();
});
builder.Services.AddSlimMessageBus(buses => buses
    .Produce<NewPostCreatedEvent>(x => x
        .Exchange("storyblog.fanout", exchangeType: ExchangeType.Fanout, durable: true)
        .RoutingKeyProvider((a, b) => "storyblog.post.created")
    )
    .Produce<PostProcessedEvent>(x => x
        .Exchange("storyblog.fanout", exchangeType: ExchangeType.Fanout, durable: true)
        .RoutingKeyProvider((a, b) => "storyblog.post.processed")
    )
    .Consume<NewCommentCreatedEvent>(x => x
        .Queue("storyblog.posts.comment.created", durable: true)
        .PerMessageScopeEnabled(enabled: true)
        .ExchangeBinding("storyblog.fanout", routingKey: "storyblog.comment.created")
        .WithConsumer<NewCommentCreatedEventConsumer>()
    )
    /*.Consume<CommentPublishedEvent>(x => x
        .Queue("storyblog.comment.published", durable: true)
        .PerMessageScopeEnabled(enabled: true)
        .ExchangeBinding("storyblog.direct", routingKey: "storyblog.comment.published")
        .WithConsumer<CommentPublishedEventHandler>()
    )
    .Consume<CommentRemovedEvent>(x => x
        .Queue("storyblog.comment.removed", durable: true)
        .PerMessageScopeEnabled(enabled: true)
        .ExchangeBinding("storyblog.direct", routingKey: "storyblog.comment.removed")
        .WithConsumer<CommentRemovedEventHandler>()
    )*/
    /*.Produce<PostPublishedEvent>(x => x
        .Exchange("storyblog.direct", ExchangeType.Direct)
        .RoutingKeyProvider((a, b) => "storyblog.post.published")
    )
    .Produce<PostDeletedMessage>(x => x
        .Exchange("storyblog.fanout", exchangeType: ExchangeType.Fanout, durable: true)
        .RoutingKeyProvider((a, b) => "storyblog.post.deleted")
    )*/
    .WithProviderRabbitMQ(rabbit =>
    {
        rabbit.ConnectionString = "amqp://admin:admin@localhost:5672";
        rabbit.UseExchangeDefaults(durable: true);
        rabbit.UseMessagePropertiesModifier((message, properties) =>
        {
            properties.ContentType = MediaTypeNames.Application.Json;
        });
    })
    .AddJsonSerializer()
    .AddAspNet()
);
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options => options
    .AddDefaultPolicy(policy => policy
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    )
);
builder.Services.AddStoryBlogAuthentication();
builder.Services.AddControllers();
builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0, "alpha");
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = false;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVVV";
        options.SubstituteApiVersionInUrl = true;
        options.SubstitutionFormat = "VVVV";
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

var app = builder.Build();

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
    .UseWebSockets()
    .UseCors()
    .UseAuthentication()
    .UseAuthorization()
    ;

app
    .MapControllers()
    .WithOpenApi()
    ;

await app.RunAsync();