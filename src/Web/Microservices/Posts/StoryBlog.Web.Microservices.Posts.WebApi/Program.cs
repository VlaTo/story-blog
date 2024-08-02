using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using SlimMessageBus.Host;
using SlimMessageBus.Host.RabbitMQ;
using SlimMessageBus.Host.Serialization.SystemTextJson;
using StoryBlog.Web.Common;
using StoryBlog.Web.Identity.DependencyInjection.Extensions;
using StoryBlog.Web.Microservices.Comments.Events;
using StoryBlog.Web.Microservices.Posts.Application.Contexts;
using StoryBlog.Web.Microservices.Posts.Application.Extensions;
using StoryBlog.Web.Microservices.Posts.Events;
using StoryBlog.Web.Microservices.Posts.Infrastructure.Extensions;
using StoryBlog.Web.Microservices.Posts.WebApi.Configuration;
using StoryBlog.Web.Microservices.Posts.WebApi.Core;
using StoryBlog.Web.Microservices.Posts.WebApi.Extensions;
using StoryBlog.Web.Microservices.Posts.WebApi.MessageBus.Consumers;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.Tracing;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;

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

var swaggerGeneratorConfigurator = new SwaggerGeneratorConfigurator();

builder.Services.AddSwaggerGen(swaggerGeneratorConfigurator.ConfigureSwaggerOptions);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    swaggerGeneratorConfigurator.ApiVersionDescriptionProvider = apiVersionDescriptionProvider;

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName
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

internal class SwaggerGeneratorConfigurator
{
    public IApiVersionDescriptionProvider? ApiVersionDescriptionProvider
    {
        get;
        set;
    }

    public void ConfigureSwaggerOptions(SwaggerGenOptions options)
    {
        if (null == ApiVersionDescriptionProvider)
        {
            throw new Exception();
        }

        foreach (var description in ApiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
        }

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

        options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    }
    
    private static OpenApiInfo CreateVersionInfo(ApiVersionDescription desc)
    {
        var assembly = Assembly.GetEntryAssembly();
        var titleAttribute = CustomAttributes.GetCustomAttribute<AssemblyTitleAttribute>(assembly);

        var title = new StringBuilder();
        var description = String.Empty;

        if (null != titleAttribute)
        {
            var targetFrameworkAttribute = CustomAttributes.GetCustomAttribute<TargetFrameworkAttribute>(assembly);

            title.Append(titleAttribute.Title);

            if (targetFrameworkAttribute is { FrameworkDisplayName: not null })
            {
                title.AppendFormat(" ({0}).", targetFrameworkAttribute.FrameworkDisplayName);
            }
        }

        var descriptionAttribute = CustomAttributes.GetCustomAttribute<AssemblyDescriptionAttribute>(assembly);

        if (null != descriptionAttribute)
        {
            description = descriptionAttribute.Description;
        }

        var info = new OpenApiInfo
        {
            Title = title.ToString(),
            Description = description,
            Version = desc.ApiVersion.ToString()
        };

        if (desc.IsDeprecated)
        {
            info.Description += " This API version has been deprecated. Please use one of the new APIs available from the explorer.";
        }

        return info;
    }
}