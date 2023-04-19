namespace SlimMessageBus.Host.NamedPipe.Core;

internal sealed class AsyncQueue<T> : IAsyncEnumerable<T>
{
    private readonly object gate;
    private readonly List<T> items;
    private readonly Queue<TaskCompletionSource<T>> queue;

    public AsyncQueue()
    {
        gate = new object();
        items = new List<T>();
        queue = new Queue<TaskCompletionSource<T>>();
    }

    public void Enqueue(T item)
    {
        lock (gate)
        {
            if (queue.TryDequeue(out var tcs))
            {
                tcs.SetResult(item);
                return;
            }

            items.Add(item);
        }
    }

    public Task<T> DequeueAsync(CancellationToken cancellationToken = default)
    {
        lock (gate)
        {
            if (0 < items.Count)
            {
                const int index = 0;
                var item = items[index];

                items.RemoveAt(index);

                return Task.FromResult(item);
            }

            var tcs = new TaskCompletionSource<T>();

            queue.Enqueue(tcs);

            return tcs.Task.WaitAsync(cancellationToken);
        }
    }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => new AsyncEnumerator(this, cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    private sealed class AsyncEnumerator : IAsyncEnumerator<T>
    {
        private AsyncQueue<T>? queue;
        private CancellationToken cancellationToken;
        private T? current;
        private EnumeratorState state;

        public T Current
        {
            get
            {
                if (EnumeratorState.Executing != state)
                {
                    throw new InvalidOperationException();
                }

                return current!;
            }
        }

        public AsyncEnumerator(AsyncQueue<T> queue, CancellationToken cancellationToken)
        {
            this.queue = queue;
            this.cancellationToken = cancellationToken;
            state = EnumeratorState.Uninitialized;
            current = default;
        }

        public ValueTask DisposeAsync()
        {
            return DisposeAsync(EnumeratorState.Disposed != state);
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            if (state is not (EnumeratorState.Executing or EnumeratorState.Uninitialized))
            {
                throw new InvalidOperationException();
            }

            if (EnumeratorState.Uninitialized == state)
            {
                state = EnumeratorState.Executing;
            }

            try
            {
                current = await queue!.DequeueAsync(cancellationToken);
            }
            catch (TaskCanceledException)
            {
                current = default;
                state = EnumeratorState.Failed;

                return false;
            }

            return true;
        }

        private ValueTask DisposeAsync(bool dispose)
        {
            if (EnumeratorState.Disposed == state)
            {
                return ValueTask.CompletedTask;
            }

            try
            {
                if (dispose)
                {
                    queue = null;
                    current = default;
                    cancellationToken = default;
                }
            }
            finally
            {
                state = EnumeratorState.Disposed;
            }

            return ValueTask.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        private enum EnumeratorState
        {
            Failed = -1,
            Uninitialized,
            Executing,
            Disposed
        }
    }
}