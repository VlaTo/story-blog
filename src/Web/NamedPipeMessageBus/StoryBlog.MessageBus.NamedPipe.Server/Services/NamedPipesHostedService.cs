using System.Diagnostics;
using System.IO.Pipes;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace StoryBlog.MessageBus.NamedPipe.Server.Services;

internal sealed class NamedPipesHostedService : HostedServiceBase
{
    public NamedPipesHostedService(
        IHostApplicationLifetime applicationLifetime,
        ILogger<NamedPipesHostedService> logger)
        : base(applicationLifetime, logger)
    {
    }

    public override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        Logger.LogInformation("Starting Worker...");

        using (var stream = new NamedPipeServerStream("new-post", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous | PipeOptions.CurrentUserOnly))
        {
            var buffer = new byte[128];

            while (true)
            {
                await stream.WaitForConnectionAsync(cancellationToken);

                Logger.LogInformation($"Incoming connection");

                var count = await stream.ReadAsync(buffer, cancellationToken);
            }
        }
    }
}