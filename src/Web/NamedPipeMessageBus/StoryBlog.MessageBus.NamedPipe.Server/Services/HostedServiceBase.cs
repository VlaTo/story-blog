using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StoryBlog.MessageBus.NamedPipe.Server.Extensions;

namespace StoryBlog.MessageBus.NamedPipe.Server.Services;

internal abstract class HostedServiceBase : IHostedService, IDisposable
{
    private readonly IHostApplicationLifetime applicationLifetime;
    private readonly CancellationTokenSource cts;
    private Task? executingTask;

    public ILogger Logger
    {
        get;
    }

    protected HostedServiceBase(
        IHostApplicationLifetime applicationLifetime,
        ILogger logger)
    {
        this.applicationLifetime = applicationLifetime;
        cts = new CancellationTokenSource();
        Logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        executingTask = DoExecuteAsync(cts.Token);

        if (executingTask.IsCompleted)
        {
            return executingTask;
        }

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (null == executingTask)
        {
            return ;
        }

        try
        {
            cts.Cancel();
        }
        finally
        {
            await Task.WhenAny(executingTask, Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken));
        }
    }

    public abstract Task ExecuteAsync(CancellationToken cancellationToken);

    public void Dispose()
    {
        cts.Cancel();
    }

    private async Task DoExecuteAsync(CancellationToken cancellationToken)
    {
        await applicationLifetime.ApplicationStarted.WaitAsync();
        await ExecuteAsync(cancellationToken);
    }
}