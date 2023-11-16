using Microsoft.IdentityModel.Logging;
using SlimMessageBus.Host;
using SlimMessageBus.Host.Serialization.SystemTextJson;
using StoryBlog.Web.Common.Events;
using StoryBlog.Web.Identity.Extensions;
using StoryBlog.Web.Microservices.Posts.Application.Contexts;
using StoryBlog.Web.Microservices.Posts.Application.Extensions;
using StoryBlog.Web.Microservices.Posts.Application.MessageBus.Handlers;
using StoryBlog.Web.Microservices.Posts.Infrastructure.Extensions;
using StoryBlog.Web.Microservices.Posts.WebApi.Configuration;
using StoryBlog.Web.Microservices.Posts.WebApi.Core;
using StoryBlog.Web.Microservices.Posts.WebApi.Extensions;
using System.Diagnostics.Tracing;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using SlimMessageBus.Host.RabbitMQ;
using StoryBlog.Web.MessageHub.Server.Extensions;
using StoryBlog.Web.MessageHub.Services;

var builder = WebApplication.CreateBuilder(args);

IdentityModelEventSource.Logger.LogLevel = EventLevel.Verbose;
IdentityModelEventSource.ShowPII = true;

builder.Configuration.AddJsonFile("appsettings.authentication.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile("appsettings.dbconnection.json", optional: true, reloadOnChange: true);

builder.Services.AddInfrastructureServices();
builder.Services.AddInfrastructureDbContext(builder.Configuration, "Database");
builder.Services.AddScoped<ILocationProvider, AspNetCoreLocationProvider>();
builder.Services.AddApplicationServices();
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
    .AddChildBus("default", bus =>
    {
        bus.Consume<NewCommentCreatedEvent>(x => x
            .Queue("storyblog.comment.created", durable: true)
            .PerMessageScopeEnabled(enabled: true)
            .ExchangeBinding("amq.topic", routingKey: "storyblog.comment.created")
            .WithConsumer<NewCommentCreatedEventHandler>()
        );
        bus.Produce<NewPostCreatedEvent>(x => x
            .Exchange("amp.topic", durable: true)
            .RoutingKeyProvider((a, b) => "storyblog.post.created")
        );
    })
    .WithProviderRabbitMQ(rabbit =>
    {
        rabbit.ConnectionString = "amqp://admin:admin@localhost:5672";
    })
    .AddJsonSerializer()
    .AddAspNet()
);
/*builder.Services.AddSlimMessageBus(buses => buses
    .WithProviderRabbitMQ(rabbit =>
    {
        rabbit.ConnectionFactory.HostName = "localhost";
        rabbit.ConnectionFactory.Port = 5672;
        rabbit.ConnectionFactory.UserName = "guest";
        rabbit.ConnectionFactory.Password = "guest";
        rabbit.ConnectionFactory.Ssl.Enabled = false;

        rabbit.UseExchangeDefaults(durable: true);
        rabbit.UseQueueDefaults(durable: true);
        rabbit.UseMessagePropertiesModifier((o, properties) =>
        {
            properties.ContentType = MediaTypeNames.Application.Json;
        });
    })
    .Produce<BlogPostEvent>(x => x
        .Exchange("blog.created")
    )
    .AddJsonSerializer()
    .AddAspNet()
);*/
builder.Services.AddHttpContextAccessor();
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
builder.Services.AddMessageHub(options =>
{
    options.Path = "/notification";
    options.Serializer = new JsonHubMessageSerializer();

    options.Channel("Test", channel =>
    {
        channel.AddHubMessageHandlers();
    });
});
builder.Services.AddControllers();
builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0, "alpha");
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
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
    //.UseApiVersioning()
    .UseCors()
    .UseAuthentication()
    .UseAuthorization()
    .UseMessageHub()
    ;
app
    .MapControllers()
    .WithOpenApi();

await app.RunAsync();