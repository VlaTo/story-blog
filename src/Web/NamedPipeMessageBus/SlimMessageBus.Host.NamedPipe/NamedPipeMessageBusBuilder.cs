namespace SlimMessageBus.Host.NamedPipe;

public class NamedPipeMessageBusBuilder : MessageBusBuilder
{
    private readonly MessageBusBuilder messageBusBuilder;

    public NamedPipeMessageBusBuilder(MessageBusBuilder messageBusBuilder)
    {
        this.messageBusBuilder = messageBusBuilder;
    }
}