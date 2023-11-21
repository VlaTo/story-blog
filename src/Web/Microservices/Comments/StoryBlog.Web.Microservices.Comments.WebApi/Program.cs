using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using SlimMessageBus.Host;
using SlimMessageBus.Host.RabbitMQ;
using SlimMessageBus.Host.Serialization.SystemTextJson;
using StoryBlog.Web.Common.Events;
using StoryBlog.Web.Identity.Extensions;
using StoryBlog.Web.Microservices.Comments.Application.Contexts;
using StoryBlog.Web.Microservices.Comments.Application.Extensions;
using StoryBlog.Web.Microservices.Comments.Application.MessageBus.Handlers;
using StoryBlog.Web.Microservices.Comments.Infrastructure.Extensions;
using StoryBlog.Web.Microservices.Comments.WebApi.Configuration;
using StoryBlog.Web.Microservices.Comments.WebApi.Core;
using StoryBlog.Web.Microservices.Comments.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.authentication.json", optional: true);
builder.Configuration.AddJsonFile("appsettings.dbconnection.json", optional: true, reloadOnChange: true);

builder.Services.AddInfrastructureServices();
builder.Services.AddInfrastructureDbContext(builder.Configuration, "Database");
builder.Services.AddScoped<ILocationProvider, AspNetCoreLocationProvider>();
builder.Services
    .AddOptions<CommentLocationProviderOptions>()
    .BindConfiguration(CommentLocationProviderOptions.SectionName);
builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssembly(typeof(ICommentsDbContext).Assembly);
});
builder.Services.AddAutoMapper(configuration =>
{
    configuration.AddApplicationMappingProfiles();
    configuration.AddWebApiMappingProfiles();
});
builder.Services.AddSlimMessageBus(buses => buses
    .AddChildBus("default", bus => bus
        .Consume<NewPostCreatedEvent>(x => x
            .Queue("storyblog.post.created", durable: true)
            .PerMessageScopeEnabled(enabled: true)
            .ExchangeBinding("amq.topic", routingKey: "storyblog.post.created")
            .WithConsumer<NewPostCreatedEventHandler>()
        )
        .Consume<PostPublishedEvent>(x => x
            .Queue("storyblog.post.published", durable: true)
            .PerMessageScopeEnabled(enabled: true)
            .ExchangeBinding("amq.topic", routingKey: "storyblog.post.published")
            .WithConsumer<PostPublishedEventHandler>()
        )
        .Consume<PostRemovedEvent>(x => x
            .Queue("storyblog.post.removed", durable: true)
            .PerMessageScopeEnabled(enabled: true)
            .ExchangeBinding("amq.topic", routingKey: "storyblog.post.removed")
            .WithConsumer<PostRemovedEventHandler>()
        )
        .Produce<NewCommentCreatedEvent>(x => x
            .Exchange("amp.topic", durable: true)
            .RoutingKeyProvider((a, b) => "storyblog.comment.created")
        )
    )
    .WithProviderRabbitMQ(rabbit =>
    {
        rabbit.ConnectionString = "amqp://admin:admin@localhost:5672";
    })
    /*
    {
    bus
            .Consume<BlogPostEvent>(x => x
                .Topic("blog-post")
                .PerMessageScopeEnabled(true)
                .WithConsumer<NewPostCreatedEventHandler>()
            );

        bus
            .Produce<BlogCommentEvent>(x => x.DefaultTopic("blog-comment"))
            .WithProviderNamedPipes(settings =>
            {
                settings.Instances = 1;
            });
    })
    .AddJsonSerializer()
    .WithProviderRabbitMQ(rabbit =>
    {
        ;
    })*/
    .AddJsonSerializer()
    .AddAspNet()
);
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
    //.UseApiVersioning()
    .UseCors()
    .UseAuthentication()
    .UseAuthorization();
app
    .MapControllers()
    .WithOpenApi();

await app.RunAsync();