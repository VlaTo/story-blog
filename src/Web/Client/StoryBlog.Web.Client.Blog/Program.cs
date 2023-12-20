using Fluxor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.Options;
using MudBlazor;
using MudBlazor.Services;
using StoryBlog.Web.Blazor.Markdown.Editor.Extensions;
using StoryBlog.Web.Client.Blog;
using StoryBlog.Web.Client.Blog.Clients;
using StoryBlog.Web.Client.Blog.Configuration;
using StoryBlog.Web.Client.Blog.Core;
using StoryBlog.Web.Client.Blog.Extensions;
using StoryBlog.Web.Client.Blog.Middlewares;
using StoryBlog.Web.Common.Identity.Permission;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Configuration.Add(new MemoryConfigurationSource
{
    InitialData = new Dictionary<string, string?>
    {
        { "key", "test" }
    }
});

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddOptions<HttpClientOptions>()
    .Configure(options =>
    {
        builder.Configuration.Bind(HttpClientOptions.SectionName, options);
    });
builder.Services.AddOidcAuthentication(options =>
{
    // Configure your authentication provider options here.
    // For more information, see https://aka.ms/blazor-standalone-auth
    builder.Configuration.Bind("Oidc", options);
});
builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("Editor", policy =>
        policy
            .RequireAuthenticatedUser()
            .RequirePermissionClaim(Permissions.Blogs.Update)
    );
});
builder.Services.AddScoped<StoryBlogApiAuthorizationMessageHandler>();
builder.Services
    .AddHttpClient<PostsHttpClient>((serviceProvider, client) =>
    {
        var httpClientOptions = serviceProvider.GetRequiredService<IOptions<HttpClientOptions>>().Value;
        client.BaseAddress = new Uri(httpClientOptions.Endpoints.Posts.BasePath);
    })
    .AddHttpMessageHandler<StoryBlogApiAuthorizationMessageHandler>();
builder.Services
    .AddHttpClient<CommentsHttpClient>((serviceProvider, client) =>
    {
        var httpClientOptions = serviceProvider.GetRequiredService<IOptions<HttpClientOptions>>().Value;
        client.BaseAddress = new Uri(httpClientOptions.Endpoints.Comments.BasePath);
    })
    .AddHttpMessageHandler<StoryBlogApiAuthorizationMessageHandler>();
builder.Services.AddMudServices();
builder.Services.AddMudMarkdownServices();
builder.Services.AddMarkdownEditor(options =>
{
    ;
});
builder.Services.AddFluxor(options =>
{
    options
        .ScanAssemblies(typeof(App).Assembly)
        .AddMiddleware<FluxorLoggingMiddleware>();
});
builder.Services.AddLocalization();

var host = builder.Build();

await host.RunAsync();
