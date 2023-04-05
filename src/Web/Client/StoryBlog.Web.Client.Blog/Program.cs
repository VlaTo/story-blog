using Fluxor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration.Memory;
using MudBlazor;
using MudBlazor.Services;
using StoryBlog.Web.Client.Blog;
using StoryBlog.Web.Client.Blog.Clients;
using StoryBlog.Web.Client.Blog.Clients.Interfaces;
using StoryBlog.Web.Client.Blog.Configuration;
using StoryBlog.Web.Client.Blog.Middlewares;

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

builder.Services.AddScoped<IPostsClient, PostsHttpClient>();
builder.Services.AddScoped<ICommentsClient, CommentsHttpClient>();
builder.Services.AddScoped<ISlugClient, SlugHttpClient>();

builder.Services.AddMudServices();
builder.Services.AddMudMarkdownServices();
builder.Services.AddFluxor(options =>
{
    options
        .ScanAssemblies(typeof(App).Assembly)
        .AddMiddleware<FluxorLoggingMiddleware>();
});
builder.Services.AddScoped(provider => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

var host = builder.Build();

await host.RunAsync();
