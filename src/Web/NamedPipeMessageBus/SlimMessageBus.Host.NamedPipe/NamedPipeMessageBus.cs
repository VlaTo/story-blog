using SlimMessageBus.Host.NamedPipe.Core;
using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;

namespace SlimMessageBus.Host.NamedPipe
{
    public sealed class NamedPipeMessageBus : MessageBusBase<NamedPipeMessageBusSettings>
    {
        private readonly List<NamedPipeClientStreamMessageProcessor> consumers;
        private readonly List<NamedPipeServerStreamMessageProcessor> producers;
        private readonly Dictionary<string, List<NamedPipeServerStreamMessageProcessor>> connectedProducers;
        private readonly object gate;
        private Task? executingTask;

        public NamedPipeMessageBus(
            MessageBusSettings settings,
            NamedPipeMessageBusSettings providerSettings)
            : base(settings, providerSettings)
        {
            consumers = new List<NamedPipeClientStreamMessageProcessor>();
            producers = new List<NamedPipeServerStreamMessageProcessor>();
            connectedProducers = new Dictionary<string, List<NamedPipeServerStreamMessageProcessor>>();
            gate = new object();

            OnBuildProvider();
        }

        internal void OnServerStreamConnected(NamedPipeServerStreamMessageProcessor processor)
        {
            lock (gate)
            {
                var key = processor.Path!;

                if (false == connectedProducers.TryGetValue(key, out var collection))
                {
                    collection = new List<NamedPipeServerStreamMessageProcessor>();
                    connectedProducers.Add(key, collection);
                }

                collection.Add(processor);
            }
        }

        internal void OnServerStreamDisconnected(NamedPipeServerStreamMessageProcessor processor)
        {
            lock (gate)
            {
                var key = processor.Path!;

                if (connectedProducers.TryGetValue(key, out var collection))
                {
                    if (collection.Remove(processor) && 0 == collection.Count)
                    {
                        connectedProducers.Remove(key);
                    }
                }
            }
        }

        protected override async Task ProduceToTransport(
            object message,
            string path,
            byte[] messagePayload,
            IDictionary<string, object>? messageHeaders = null,
            CancellationToken cancellationToken = default)
        {
            var publishers = Array.Empty<NamedPipeServerStreamMessageProcessor>();

            lock (gate)
            {
                if (connectedProducers.TryGetValue(path, out var collection))
                {
                    publishers = collection.ToArray();
                }
            }

            if (0 < publishers.Length)
            {
                var messageType = message.GetType();
                var bytes = Serializer.Serialize(messageType, message);
                var headers = null != messageHeaders
                    ? (messageHeaders as IReadOnlyDictionary<string, object>) ??
                      new ReadOnlyDictionary<string, object>(messageHeaders)
                    : null;

                for (var index = 0; index < publishers.Length; index++)
                {
                    await publishers[index].PublishAsync(bytes, cancellationToken);
                }
            }
        }

        protected override async Task OnStart()
        {
            await base.OnStart();

            var tasks = new List<Task>();

            foreach (var processor in producers.ToArray())
            {
                tasks.Add(processor.ExecuteAsync(CancellationToken.None, Settings.ServiceProvider));
            }

            if (Settings.AutoStartConsumers)
            {
                foreach (var processor in consumers.ToArray())
                {
                    tasks.Add(processor.ExecuteAsync(CancellationToken.None, Settings.ServiceProvider));
                }
            }

            executingTask = Task.WhenAll(tasks);
        }

        protected override async Task OnStop()
        {
            await base.OnStop();
        }

        protected override ValueTask DisposeAsyncCore()
        {
            return base.DisposeAsyncCore();
        }

        protected override void Build()
        {
            base.Build();

            for (var index = 0; index < Settings.Consumers.Count; index++)
            {
                var consumerSettings = Settings.Consumers[index];
                consumers.Add(ActivatorUtilities.CreateInstance<NamedPipeClientStreamMessageProcessor>(
                    Settings.ServiceProvider,
                    consumerSettings,
                    this
                ));
            }

            for (var index = 0; index < Settings.Producers.Count; index++)
            {
                var producerSettings = Settings.Producers[index];

                for (var instance = 0; instance < ProviderSettings.Instances; instance++)
                {
                    producers.Add(ActivatorUtilities.CreateInstance<NamedPipeServerStreamMessageProcessor>(
                        Settings.ServiceProvider,
                        instance,
                        producerSettings,
                        this
                    ));
                }
            }
        }
    }
}