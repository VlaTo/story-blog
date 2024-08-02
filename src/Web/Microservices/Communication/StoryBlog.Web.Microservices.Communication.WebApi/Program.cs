using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.IdentityModel.Logging;
using SlimMessageBus.Host;
using SlimMessageBus.Host.RabbitMQ;
using SlimMessageBus.Host.Serialization.SystemTextJson;
using StoryBlog.Web.MessageHub.Server.Extensions;
using StoryBlog.Web.MessageHub.Services;
using StoryBlog.Web.Microservices.Comments.Events;
using StoryBlog.Web.Microservices.Communication.Application.Contexts;
using StoryBlog.Web.Microservices.Communication.Application.Extensions;
using StoryBlog.Web.Microservices.Communication.Infrastructure.Extensions;
using StoryBlog.Web.Microservices.Communication.WebApi.Configuration;
using StoryBlog.Web.Microservices.Communication.WebApi.Extensions;
using StoryBlog.Web.Microservices.Communication.WebApi.MessageBus.Consumers;
using StoryBlog.Web.Microservices.Posts.Events;
using System.Diagnostics.Tracing;
using System.Net.Mime;
using StoryBlog.Web.Identity.DependencyInjection.Extensions;
using StoryBlog.Web.Microservices.Communication.MessageHub.Messages;
using StoryBlog.Web.Microservices.Communication.WebApi.MessageHub.Handlers;

var builder = WebApplication.CreateBuilder(args);

IdentityModelEventSource.Logger.LogLevel = EventLevel.Verbose;
IdentityModelEventSource.ShowPII = true;

builder.Configuration.AddJsonFile("appsettings.authentication.json", optional: true);
builder.Configuration.AddJsonFile("appsettings.dbconnection.json", optional: true, reloadOnChange: true);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddWebServices();

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssembly(typeof(ICommunicationDbContext).Assembly);
});
builder.Services.AddAutoMapper(configuration =>
{
    configuration.AddApplicationMappingProfiles();
    configuration.AddWebApiMappingProfiles();
});
builder.Services.AddSlimMessageBus(buses => buses
    .Consume<NewPostCreatedEvent>(x => x
        .Queue("storyblog.communication.post.created", durable: true)
        .PerMessageScopeEnabled(enabled: true)
        .ExchangeBinding("storyblog.fanout", routingKey: "storyblog.post.created")
        .WithConsumer<NewPostCreatedEventConsumer>()
    )
    .Consume<NewCommentCreatedEvent>(x => x
        .Queue("storyblog.communication.comment.created", durable: true)
        .PerMessageScopeEnabled(enabled: true)
        .ExchangeBinding("storyblog.fanout", routingKey: "storyblog.comment.created")
        .WithConsumer<NewCommentCreatedEventConsumer>()
    )
    /*.Consume<PostDeletedMessage>(x => x
        .Queue("storyblog.communication.post.deleted", durable: true)
        .PerMessageScopeEnabled(enabled: true)
        .ExchangeBinding("storyblog.direct", routingKey: "storyblog.post.deleted")
        .WithConsumer<PostDeletedMessageConsumer>()
    )

    .Consume<PostPublishedEvent>(x => x
        .Queue("storyblog.post.published", durable: true)
        .PerMessageScopeEnabled(enabled: true)
        .ExchangeBinding("storyblog.direct", routingKey: "storyblog.post.published")
        .WithConsumer<PostPublishedEventHandler>()
    )
    .Consume<PostRemovedEvent>(x => x
        .Queue("storyblog.post.removed", durable: true)
        .PerMessageScopeEnabled(enabled: true)
        .ExchangeBinding("storyblog.direct", routingKey: "storyblog.post.removed")
        .WithConsumer<PostRemovedEventHandler>()
    )
    .Produce<NewCommentCreatedEvent>(x => x
        .Exchange("storyblog.direct", exchangeType: ExchangeType.Direct)
        .RoutingKeyProvider((a, b) => "storyblog.comment.created")
    )
    .Produce<CommentPublishedEvent>(x => x
        .Exchange("storyblog.direct", exchangeType: ExchangeType.Direct)
        .RoutingKeyProvider((a, b) => "storyblog.comment.published")
    )
    .Produce<CommentRemovedEvent>(x => x
        .Exchange("storyblog.direct", exchangeType: ExchangeType.Direct)
        .RoutingKeyProvider((a, b) => "storyblog.comment.removed")
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
builder.Services.AddMessageHub(options =>
{
    options.Path = "/notification";
    options.Serializer = new JsonHubMessageSerializer();

    options.Channel("Test", channel =>
        channel
            .AddMessage<ClientActionRequestedHubMessage>()
            .WithHandler<ClientActionRequestedEventHandler>()
    );
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
    .UseWebSockets()
    .UseCors()
    .UseAuthentication()
    .UseAuthorization()
    .UseMessageHub()
    ;
app
    .MapControllers()
    .WithOpenApi()
    ;

await app.RunAsync();