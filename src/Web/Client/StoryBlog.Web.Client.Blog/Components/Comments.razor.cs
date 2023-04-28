using Microsoft.AspNetCore.Components;
using MudBlazor.Utilities;
using StoryBlog.Web.Client.Blog.Core;
using StoryBlog.Web.Client.Blog.Models;

namespace StoryBlog.Web.Client.Blog.Components;

/// <summary>
/// 
/// </summary>
public sealed class FetchCommentsEventArgs : EventArgs
{
    public Guid PostKey
    {
        get;
    }

    public Guid ParentKey
    {
        get;
    }

    public FetchCommentsEventArgs(Guid postKey, Guid parentKey)
    {
        PostKey = postKey;
        ParentKey = parentKey;
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

    Task ICommentsCoordinator.FetchCommentsAsync(Guid postKey, Guid parentKey)
    {
        var handler = OnFetchComments;
        return handler.InvokeAsync(new FetchCommentsEventArgs(postKey, parentKey));
    }

    Task ICommentsCoordinator.PublishReplyAsync(Guid postKey, Guid parentKey, string reply)
    {
        return Task.Delay(TimeSpan.FromSeconds(5.0d));
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