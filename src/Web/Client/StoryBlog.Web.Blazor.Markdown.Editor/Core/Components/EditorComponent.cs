using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace StoryBlog.Web.Blazor.Markdown.Editor.Core.Components;

public class EditorComponent : ComponentBase, IMarkdownEditorContext, IAsyncDisposable
{
    private readonly List<SubscriptionToken> subscriptions;

    [Parameter]
    public bool AutoFocus
    {
        get;
        set;
    }

    protected bool FirstRender
    {
        get;
        private set;
    }

    protected bool IsDirty
    {
        get; 
        private protected set;
    }

    protected EditorComponent()
    {
        FirstRender = false;
        IsDirty = false;
        subscriptions = new List<SubscriptionToken>();
    }

    public virtual ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        FirstRender = firstRender;
        
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            for (var index = 0; index < subscriptions.Count; index++)
            {
                var observer = subscriptions[index].Observer;
                observer!.OnInitialized();
            }
        }
    }

    protected virtual ValueTask OnBlurAsync(FocusEventArgs e)
    {
        return ValueTask.CompletedTask;
    }

    protected virtual ValueTask OnFocusAsync(FocusEventArgs e)
    {
        return ValueTask.CompletedTask;
    }

    #region IMarkdownEditorContext

    IDisposable IMarkdownEditorContext.Subscribe(IMarkdownEditorObserver observer)
    {
        var index = FindObserverIndex(observer);

        if (0 > index)
        {
            index = subscriptions.Count;
            subscriptions.Add(new SubscriptionToken(this, observer));
        }

        return subscriptions[index];
    }

    bool IMarkdownEditorContext.IsActionEnabled(EditorAction action)
    {
        return EditorAction.Undo != action;
    }

    #endregion

    private void RemoveObserver(IMarkdownEditorObserver observer)
    {
        var index = FindObserverIndex(observer);

        if (-1 < index)
        {
            subscriptions.RemoveAt(index);
        }
    }

    private int FindObserverIndex(IMarkdownEditorObserver observer)
    {
        return subscriptions.FindIndex(x => ReferenceEquals(x.Observer, observer));
    }

    private sealed class SubscriptionToken : IDisposable
    {
        private bool disposed;

        public EditorComponent? Owner
        {
            get;
            private set;
        }

        public IMarkdownEditorObserver? Observer
        {
            get;
            private set;
        }

        public SubscriptionToken(EditorComponent owner, IMarkdownEditorObserver observer)
        {
            Owner = owner;
            Observer = observer;
        }

        public void Dispose()
        {
            Dispose(true);
        }

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
                    Owner?.RemoveObserver(Observer!);
                    Owner = null;
                    Observer = null;
                }
            }
            finally
            {
                disposed = true;
            }
        }
    }
}