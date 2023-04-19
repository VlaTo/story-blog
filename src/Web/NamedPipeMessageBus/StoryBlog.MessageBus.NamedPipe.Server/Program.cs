using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StoryBlog.MessageBus.NamedPipe.Server.Services;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((context, services) =>
{
    services.AddHostedService<NamedPipesHostedService>();
});

builder.UseConsoleLifetime();

var host = builder.Build();

await host.RunAsync();