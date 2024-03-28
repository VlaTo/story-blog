using Microsoft.AspNetCore.Components;
using MudBlazor.Utilities;
using StoryBlog.Web.Client.Core;

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
    public Guid? ParentKey
    {
        get;
    }

    public string CorrelationKey
    {
        get;
    }

    public string Reply
    {
        get;
    }

    public PublishReplyEventArgs(Guid? parentKey, string reply, string correlationKey)
    {
        ParentKey = parentKey;
        Reply = reply;
        CorrelationKey = correlationKey;
    }
}

/// <summary>
/// 
/// </summary>
public partial class Comments : ICommentsCoordinator
{
    public const string CoordinatorParameterName = "CommentsCoordinator";
    
    private readonly List<Disposable<ICommentsObserver>> disposables;
    
    protected string Classname => new CssBuilder("storyblog-blog-comments")
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
    public EventCallback<FetchCommentsEventArgs> FetchComments
    {
        get;
        set;
    }

    [Parameter]
    public EventCallback<PublishReplyEventArgs> PublishReply
    {
        get;
        set;
    }

    public Comments()
    {
        disposables = new List<Disposable<ICommentsObserver>>();
    }

    #region ICommentCoordinator implementation

    public IDisposable Subscribe(ICommentsObserver observer)
    {
        var index = disposables.FindIndex(x => ReferenceEquals(x.State, observer));

        if (0 > index)
        {
            index = disposables.Count;
            disposables.Add(new Disposable<ICommentsObserver>(observer, RemoveSubscription));
        }

        return disposables[index];
    }

    public void NotifyReplyComposerOpened(ICommentsObserver observer)
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

    public Task FetchCommentsAsync(Guid parentKey)
    {
        var handler = FetchComments;
        return handler.InvokeAsync(new FetchCommentsEventArgs(parentKey));
    }

    public async Task PublishReplyAsync(Guid parentKey, string reply, string correlationKey)
    {
        var handler = PublishReply;

        if (handler.HasDelegate)
        {
            await handler.InvokeAsync(new PublishReplyEventArgs(parentKey, reply, correlationKey));
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