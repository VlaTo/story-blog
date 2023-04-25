using System.IO.Pipes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SlimMessageBus.Host.NamedPipe.Extensions;

namespace SlimMessageBus.Host.NamedPipe.Core;

internal sealed class NamedPipeClientStreamMessageProcessor : IAsyncDisposable
{
    private readonly NamedPipeMessageBus bus;
    private readonly IMessageBufferPool bufferPool;
    private readonly ILogger<NamedPipeClientStreamMessageProcessor> logger;

    public ConsumerSettings? ConsumerSettings
    {
        get;
    }

    public string PipeName => NamedPipeNamePartition.GetPipeName(ConsumerSettings?.Path);

    public string? Path => ConsumerSettings?.Path;

    public NamedPipeClientStreamMessageProcessor(
        ConsumerSettings? consumerSettings,
        NamedPipeMessageBus bus,
        IMessageBufferPool bufferPool,
        ILogger<NamedPipeClientStreamMessageProcessor> logger)
    {
        ConsumerSettings = consumerSettings;
        this.bus = bus;
        this.bufferPool = bufferPool;
        this.logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default, IServiceProvider? serviceProvider = null)
    {
        logger.LogExecuteStarting(Path, PipeName);

        try
        {
            using (var clientStream = new NamedPipeClientStream(".", PipeName, PipeDirection.InOut, PipeOptions.Asynchronous))
            {
                await clientStream.ConnectAsync(cancellationToken);
                
                logger.LogClientStreamConnected();

                if (false == clientStream.IsConnected)
                {
                    throw new ApplicationException();
                }

                var header = new Memory<byte>(new byte[4]);

                while (true)
                {
                    await clientStream.ReadExactlyAsync(header, cancellationToken);

                    var length = BitConverter.ToInt32(header.Span);

                    using (var payload = bufferPool.Acquire(length))
                    {
                        var buffer = payload.Buffer.Slice(0, payload.Length);

                        await clientStream.ReadExactlyAsync(buffer, cancellationToken);

                        switch (ConsumerSettings?.ConsumerMode ?? ConsumerMode.Consumer)
                        {
                            case ConsumerMode.Consumer:
                            {
                                var message = bus.Serializer.Deserialize(ConsumerSettings?.MessageType, buffer.ToArray());
                                var method = ConsumerSettings?.ConsumerMethod;

                                if (null != method)
                                {
                                    var scope = CreateAsyncServiceScope(serviceProvider!);
                                    var consumer = ActivatorUtilities.GetServiceOrCreateInstance(scope.ServiceProvider, ConsumerSettings!.ConsumerType);

                                    if (null == consumer)
                                    {
                                        throw new ApplicationException();
                                    }

                                    await method.Invoke(consumer, message);
                                }

                                break;
                            }
                            case ConsumerMode.RequestResponse:
                            {

                                break;
                            }
                        }
                    }
                }
            }
        }
        catch (OperationCanceledException)
        {
            ;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, nameof(NamedPipeClientStreamMessageProcessor));
        }
        finally
        {
            ;
        }
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    private AsyncServiceScope CreateAsyncServiceScope(IServiceProvider serviceProvider)
    {
        if (ConsumerSettings?.IsMessageScopeEnabled ?? false)
        {
            return serviceProvider.CreateAsyncScope();
        }

        return new AsyncServiceScope(new EmptyServiceScope(serviceProvider));
    }

    /// <summary>
    /// 
    /// </summary>
    private sealed class EmptyServiceScope : IServiceScope
    {
        public IServiceProvider ServiceProvider
        {
            get;
        }

        public EmptyServiceScope(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public void Dispose()
        {
            ;
        }
    }
}