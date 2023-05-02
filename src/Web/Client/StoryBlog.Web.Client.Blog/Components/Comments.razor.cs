using Microsoft.AspNetCore.Components;
using MudBlazor.Utilities;
using StoryBlog.Web.Client.Blog.Core;

namespace StoryBlog.Web.Client.Blog.Components;

/// <summary>
/// 
/// </summary>
public sealed class FetchCommentsEventArgs : EventArgs
{
    public Guid ParentKey
    {
        get;
    }

    public FetchCommentsEventArgs(Guid parentKey)
    {
        ParentKey = parentKey;
    }
}

/// <summary>
/// 
/// </summary>
public sealed class PublishReplyEventArgs : EventArgs
{
    public Guid ParentKey
    {
        get;
    }

    public string CorrelationId
    {
        get;
    }

    public string Reply
    {
        get;
    }

    public PublishReplyEventArgs(Guid parentKey, string reply, string correlationId)
    {
        ParentKey = parentKey;
        Reply = reply;
        CorrelationId = correlationId;
    }
}

/// <summary>
/// 
/// </summary>
public partial class Comments : ICommentsCoordinator
{
    public const string CoordinatorParameterName = "CommentsCoordinator";
    
    private readonly List<Disposable<ICommentsObserver>> disposables;
    
    protected string Classname => new CssBuilder("storyblog-blog-comment")
        .AddClass(Class)
        .Build();

    [Parameter]
    public string? Class
    {
        get;
        set;
    }

    [Parameter]
    public RenderFragment ChildContent
    {
        get;
        set;
    }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object?> UserAttributes
    {
        get;
        set;
    }

    [Parameter]
    public EventCallback<FetchCommentsEventArgs> OnFetchComments
    {
        get;
        set;
    }

    [Parameter]
    public EventCallback<PublishReplyEventArgs> OnPublishReply
    {
        get;
        set;
    }

    public Comments()
    {
        disposables = new List<Disposable<ICommentsObserver>>();
    }

    #region ICommentCoordinator implementation

    IDisposable ICommentsCoordinator.Subscribe(ICommentsObserver observer)
    {
        var index = disposables.FindIndex(x => ReferenceEquals(x.State, observer));

        if (0 > index)
        {
            index = disposables.Count;
            disposables.Add(new Disposable<ICommentsObserver>(observer, RemoveSubscription));
        }

        return disposables[index];
    }

    void ICommentsCoordinator.NotifyReplyComposerOpened(ICommentsObserver observer)
    {
        for (var index = 0; index < disposables.Count; index++)
        {
            var currentObserver = disposables[index].State;

            if (ReferenceEquals(currentObserver, observer))
            {
                continue;
            }

            currentObserver.OnOpenReplyComposer();
        }
    }

    Task ICommentsCoordinator.FetchCommentsAsync(Guid parentKey)
    {
        var handler = OnFetchComments;
        return handler.InvokeAsync(new FetchCommentsEventArgs(parentKey));
    }

    async Task ICommentsCoordinator.PublishReplyAsync(Guid parentKey, string reply)
    {
        var handler = OnPublishReply;

        if (handler.HasDelegate)
        {
            var correlationId = Guid.NewGuid().ToString("N");
            await handler.InvokeAsync(new PublishReplyEventArgs(parentKey, reply, correlationId));
        }
    }

    #endregion

    private void RemoveSubscription(ICommentsObserver observer)
    {
        var index = disposables.FindIndex(x => ReferenceEquals(x.State, observer));

        if (-1 < index)
        {
            disposables.RemoveAt(index);
        }
    }
}