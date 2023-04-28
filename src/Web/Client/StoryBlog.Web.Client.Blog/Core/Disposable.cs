namespace StoryBlog.Web.Client.Blog.Core;

internal abstract class DisposableBase : IDisposable
{
    private bool disposed;

    protected DisposableBase()
    {
    }

    public void Dispose() => Dispose(true);

    protected abstract void DoDispose();

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
                DoDispose();
            }
        }
        finally
        {
            disposed = true;
        }
    }
}

/// <summary>
/// 
/// </summary>
internal sealed class Disposable : DisposableBase
{
    private readonly Action disposeAction;

    public Disposable(Action disposeAction)
    {
        this.disposeAction = disposeAction;
    }

    protected override void DoDispose() => disposeAction.Invoke();
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
internal sealed class Disposable<T> : DisposableBase
{
    private readonly Action<T> disposeAction;

    public T State
    {
        get;
    }

    public Disposable(T state, Action<T> disposeAction)
    {
        State = state;
        this.disposeAction = disposeAction;
    }

    protected override void DoDispose() => disposeAction.Invoke(State);
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
internal sealed class Disposable<T1, T2> : DisposableBase
{
    private readonly Action<T1, T2> disposeAction;

    public T1 Item0
    {
        get;
    }

    public T2 Item1
    {
        get;
    }

    public Disposable(T1 item0, T2 item1, Action<T1, T2> disposeAction)
    {
        Item0 = item0;
        Item1 = item1;
        this.disposeAction = disposeAction;
    }

    protected override void DoDispose() => disposeAction.Invoke(Item0, Item1);
}