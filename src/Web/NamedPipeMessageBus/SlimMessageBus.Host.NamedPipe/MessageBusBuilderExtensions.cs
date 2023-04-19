namespace SlimMessageBus.Host.NamedPipe;

public static class MessageBusBuilderExtensions
{
    public static NamedPipeMessageBusBuilder WithProviderNamedPipes(this MessageBusBuilder builder, Action<NamedPipeMessageBusSettings>? configure = null)
    {
        var providerSettings = new NamedPipeMessageBusSettings();

        if (null != configure)
        {
            configure.Invoke(providerSettings);
        }

        var messageBusBuilder = builder.WithProvider(
            settings => new NamedPipeMessageBus(settings, providerSettings)
        );
        
        return new NamedPipeMessageBusBuilder(messageBusBuilder);
    }
}