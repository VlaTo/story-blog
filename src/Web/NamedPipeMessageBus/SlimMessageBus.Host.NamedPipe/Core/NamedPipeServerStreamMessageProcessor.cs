using System.IO.Pipes;
using Microsoft.Extensions.Logging;
using SlimMessageBus.Host.NamedPipe.Extensions;

namespace SlimMessageBus.Host.NamedPipe.Core;

internal sealed class NamedPipeServerStreamMessageProcessor : IAsyncDisposable
{
    private readonly NamedPipeMessageBus bus;
    private readonly ILogger<NamedPipeServerStreamMessageProcessor> logger;
    private readonly AsyncQueue<byte[]> queue;
    private bool disposed;

    public int Number
    {
        get;
    }

    public ProducerSettings? ProducerSettings
    {
        get;
    }

    public string PipeName => NamedPipeNamePartition.GetPipeName(ProducerSettings?.DefaultPath);

    public string? Path => ProducerSettings?.DefaultPath;

    public Type? MessageType => ProducerSettings?.MessageType;

    public NamedPipeServerStreamMessageProcessor(
        int number,
        ProducerSettings? producerSettings,
        NamedPipeMessageBus bus,
        ILogger<NamedPipeServerStreamMessageProcessor> logger)
    {
        this.bus = bus;
        this.logger = logger;
        Number = number;
        ProducerSettings = producerSettings;
        queue = new AsyncQueue<byte[]>();
    }

    public ValueTask PublishAsync(ReadOnlySpan<byte> message, CancellationToken cancellationToken = default)
    {
        queue.Enqueue(message.ToArray());
        return ValueTask.CompletedTask;
    }

    public ValueTask DisposeAsync() => DisposeAsync(true);

    public async Task ExecuteAsync(CancellationToken cancellationToken, IServiceProvider currentServiceProvider)
    {
        logger.LogExecuteStarting(Number, Path, PipeName);

        while (false == cancellationToken.IsCancellationRequested)
        {
            logger.LogExecuteStarted(Number);

            try
            {
                using (var stream = new NamedPipeServerStream(PipeName, PipeDirection.InOut,
                           NamedPipeServerStream.MaxAllowedServerInstances, PipeTransmissionMode.Byte))
                {
                    await stream.WaitForConnectionAsync(cancellationToken);

                    bus.OnServerStreamConnected(this);
                    logger.LogServerStreamConnected(Number, PipeName);

                    while (stream.IsConnected && false == cancellationToken.IsCancellationRequested)
                    {
                        var message = await queue.DequeueAsync(cancellationToken);

                        await stream.WriteAsync(BitConverter.GetBytes(message.Length), cancellationToken);
                        await stream.WriteAsync(message, cancellationToken);
                        await stream.FlushAsync(cancellationToken);

                        logger.LogMessageSent(Number, ProducerSettings?.MessageType, message.Length);
                    }
                }
            }
            catch (OperationCanceledException exception)
            {
                logger.LogException(Number, exception);
                break;
            }
            catch (Exception exception)
            {
                logger.LogException(Number, exception);
            }
            finally
            {
                logger.LogFinalizing(Number);
            }

            bus.OnServerStreamDisconnected(this);
            logger.LogServerStreamDisconnected(Number);
        }

        logger.LogServerStreamDone(Number);
    }

    private async ValueTask DisposeAsync(bool dispose)
    {
        if (disposed)
        {
            ;
        }

        try
        {
            if (dispose)
            {
                await Task.CompletedTask;
            }
        }
        finally
        {
            disposed = true;
        }
    }
}