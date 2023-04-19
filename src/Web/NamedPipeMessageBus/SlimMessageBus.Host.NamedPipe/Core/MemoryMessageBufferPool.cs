namespace SlimMessageBus.Host.NamedPipe.Core;

internal sealed class MemoryMessageBufferPool : IMessageBufferPool
{
    private readonly LinkedList<Memory<byte>> buffers;
    private readonly LinkedList<LeasedMessageBuffer> leased;
    private readonly object gate;

    public MemoryMessageBufferPool()
    {
        buffers = new LinkedList<Memory<byte>>();
        leased = new LinkedList<LeasedMessageBuffer>();
        gate = new object();
    }

    public IMessageBuffer Acquire(int length)
    {
        LeasedMessageBuffer buffer;

        lock (gate)
        {
            var entry = FindEntry(length);

            if (null == entry)
            {
                var memory = new Memory<byte>(new byte[length]);
                entry = new LinkedListNode<Memory<byte>>(memory);
            }
            else
            {
                buffers.Remove(entry);
            }

            buffer = new LeasedMessageBuffer(this, entry, length);

            leased.AddFirst(buffer);
        }

        return buffer;
    }

    private LinkedListNode<Memory<byte>>? FindEntry(int length)
    {
        for (var node = buffers.First; null != node; node = node.Next)
        {
            if (length >= node.Value.Length)
            {
                return node;
            }
        }

        return null;
    }

    private void Release(LeasedMessageBuffer buffer)
    {
        lock (gate)
        {
            if (false == leased.Remove(buffer))
            {
                throw new Exception();
            }

            var length = buffer.Buffer.Length;

            for (var entry = buffers.First; null != entry; entry = entry.Next)
            {
                if (length < entry.Value.Length)
                {
                    continue;
                }

                buffers.AddBefore(entry, buffer.Buffer);

                return ;
            }

            buffers.AddLast(buffer.Buffer);
        }
    }

    private sealed class LeasedMessageBuffer : IMessageBuffer
    {
        private MemoryMessageBufferPool? pool;
        private bool disposed;

        public Memory<byte> Buffer => Entry.Value;

        public LinkedListNode<Memory<byte>> Entry
        {
            get;
        }

        public int Length
        {
            get;
        }
        
        public LeasedMessageBuffer(MemoryMessageBufferPool pool, LinkedListNode<Memory<byte>> entry, int length)
        {
            Entry = entry;
            Length = length;
            this.pool = pool;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool dispose)
        {
            if (disposed)
            {
                return ;
            }

            try
            {
                if (dispose)
                {
                    pool?.Release(this);
                    pool = null;
                }
            }
            finally
            {
                disposed = true;
            }
        }
    }
}