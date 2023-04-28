using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using MudBlazor.Utilities;
using StoryBlog.Web.Client.Blog.Core;

namespace StoryBlog.Web.Client.Blog.Components;

/// <summary>
/// 
/// </summary>
public sealed class CommentReplyEventArgs : EventArgs
{
    public Guid Key
    {
        get;
    }

    public Guid PostKey
    {
        get;
    }

    public Guid? ParentKey
    {
        get;
    }

    public CommentReplyEventArgs(Guid key, Guid postKey, Guid? parentKey)
    {
        Key = key;
        PostKey = postKey;
        ParentKey = parentKey;
    }
}

/// <summary>
/// 
/// </summary>
public partial class Comment : ICommentsObserver, IDisposable
{
    private IDisposable? editorSubscription;
    private ReplyState replyState;

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
    public Guid Key
    {
        get;
        set;
    }

    [Parameter]
    public Guid? ParentKey
    {
        get;
        set;
    }

    [Parameter]
    public Guid PostKey
    {
        get;
        set;
    }

    [Parameter]
    public DateTime CreatedDateTime
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

    [Parameter]
    public bool IsCommentsExpanded
    {
        get;
        set;
    }

    public string? ReplyText
    {
        get;
        private set;
    }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object?> UserAttributes
    {
        get;
        set;
    }

    [Parameter]
    public CommentsCollection Comments
    {
        get;
        set;
    }

    [CascadingParameter(Name = Components.Comments.CoordinatorParameterName)]
    private ICommentsCoordinator Coordinator
    {
        get;
        set;
    }

    [Inject]
    private IStringLocalizer<Comment> Localizer
    {
        get;
        set;
    }

    public Comment()
    {
        replyState = ReplyState.Closed;
        Comments = new CommentsCollection(CommentsCollectionState.Unknown, null);
    }

    void IDisposable.Dispose()
    {
        editorSubscription?.Dispose();
        editorSubscription = null;
    }

    void ICommentsObserver.OnOpenReplyComposer()
    {
        if (ReplyState.Composing == replyState)
        {
            DoCancelReplyComposer(new MouseEventArgs());
        }
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        editorSubscription = Coordinator.Subscribe(this);
    }

    #region Reply compose callbacks

    private void DoOpenReplyComposer(MouseEventArgs _)
    {
        if (ReplyState.Composing == replyState)
        {
            return ;
        }

        replyState = ReplyState.Composing;

        StateHasChanged();

        Coordinator.NotifyReplyComposerOpened(this);
    }

    private void DoCancelReplyComposer(MouseEventArgs _)
    {
        if (ReplyState.Composing != replyState)
        {
            return ;
        }

        replyState = ReplyState.Closed;

        StateHasChanged();
    }
    
    private async Task DoSendReply(MouseEventArgs _)
    {
        if (ReplyState.Composing != replyState)
        {
            return;
        }

        replyState = ReplyState.Publishing;

        StateHasChanged();

        await Coordinator.PublishReplyAsync(PostKey, Key, ReplyText!);
        
        replyState = ReplyState.Success;
        
        StateHasChanged();
    }

    #endregion

    private async Task DoToggleComments(MouseEventArgs _)
    {
        var expand = false == IsCommentsExpanded;

        if (expand && CommentsCollectionState.Unknown == Comments.State)
        {
            await Coordinator.FetchCommentsAsync(PostKey, Key);
        }

        IsCommentsExpanded = expand;
    }

    /// <summary>
    /// 
    /// </summary>
    private enum ReplyState
    {
        Failed = -1,
        Closed,
        Composing,
        Publishing,
        Success
    }
}