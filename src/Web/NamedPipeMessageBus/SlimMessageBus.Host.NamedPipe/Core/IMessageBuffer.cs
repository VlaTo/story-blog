namespace SlimMessageBus.Host.NamedPipe.Core;

public interface IMessageBuffer : IDisposable
{
    Memory<byte> Buffer
    {
        get;
    }

    int Length
    {
        get;
    }
}