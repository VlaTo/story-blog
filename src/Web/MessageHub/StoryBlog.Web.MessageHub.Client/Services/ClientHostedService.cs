using Microsoft.Extensions.Hosting;

namespace StoryBlog.Web.Hub.Blazor.WebAssembly.Services;

public class ClientHostedService : IHostedService
{
    private readonly CancellationTokenSource cts;
    private Task? task;

    public ClientHostedService()
    {
        cts = new CancellationTokenSource();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        task = ExecuteAsync();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        cts.Cancel();
        return Task.CompletedTask;
    }

    protected async Task ExecuteAsync()
    {
        while (true)
        {
            Console.WriteLine($"Client hosted service, time: {DateTime.Now}");
            
            await Task.Delay(TimeSpan.FromSeconds(1.0d), cts.Token);
        }
    }
}