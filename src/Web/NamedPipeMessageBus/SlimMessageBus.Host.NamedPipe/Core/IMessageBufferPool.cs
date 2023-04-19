namespace SlimMessageBus.Host.NamedPipe.Core;

public interface IMessageBufferPool
{
    IMessageBuffer Acquire(int length);
}