namespace StoryBlog.Web.Client.Blog.Core;

internal sealed class CommentEditorCoordinator : ICommentEditorCoordinator
{
    private readonly List<Subscription> subscriptions;

    public CommentEditorCoordinator()
    {
        subscriptions = new List<Subscription>();
    }

    public void RaiseEditorEvent(ICommentEditor editor)
    {
        for (var index = 0; index < subscriptions.Count; index++)
        {
            var observer = subscriptions[index].Observer;

            if (null == observer)
            {
                continue;
            }

            observer.OnEditorEventRaised(editor);
        }
    }

    public IDisposable Subscribe(ICommentEditorObserver observer)
    {
        var index = subscriptions.FindIndex(x => ReferenceEquals(x.Observer, observer));

        if (0 > index)
        {
            index = subscriptions.Count;
            subscriptions.Add(new Subscription(this, observer));
        }

        return subscriptions[index];
    }

    private void RemoveSubscription(Subscription subscription)
    {
        var index = subscriptions.FindIndex(x => ReferenceEquals(x, subscription));

        if (-1< index)
        {
            subscriptions.RemoveAt(index);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private sealed class Subscription : IDisposable
    {
        private CommentEditorCoordinator? coordinator;
        private bool disposed;

        public ICommentEditorObserver? Observer
        {
            get;
        }

        public Subscription(CommentEditorCoordinator coordinator, ICommentEditorObserver observer)
        {
            this.coordinator = coordinator;
            Observer = observer;
        }

        public void Dispose() => Dispose(true);

        private void Dispose(bool dispose)
        {
            if (disposed)
            {
                return;
            }

            try
            {
                if (dispose)
                {
                    coordinator!.RemoveSubscription(this);
                    coordinator = null;
                }
            }
            finally
            {
                disposed = true;
            }
        }
    }
}