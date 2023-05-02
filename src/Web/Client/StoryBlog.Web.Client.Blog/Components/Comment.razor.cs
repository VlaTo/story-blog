using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using MudBlazor.Utilities;
using StoryBlog.Web.Client.Blog.Core;

namespace StoryBlog.Web.Client.Blog.Components;

/// <summary>
/// 
/// </summary>
public partial class Comment : ICommentsObserver, IDisposable
{
    private IDisposable? editorSubscription;

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

    [Parameter]
    public bool IsReplyComposerOpened
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
        Comments = new CommentsCollection(CommentsCollectionState.Unknown, null);
    }

    void IDisposable.Dispose()
    {
        editorSubscription?.Dispose();
        editorSubscription = null;
    }

    void ICommentsObserver.OnOpenReplyComposer()
    {
        if (IsReplyComposerOpened)
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
        if (IsReplyComposerOpened)
        {
            return ;
        }

        IsReplyComposerOpened=true;

        StateHasChanged();

        Coordinator.NotifyReplyComposerOpened(this);
    }

    private void DoCancelReplyComposer(MouseEventArgs _)
    {
        if (false == IsReplyComposerOpened)
        {
            return ;
        }

        IsReplyComposerOpened = false;

        StateHasChanged();
    }
    
    private async Task DoSendReply(MouseEventArgs _)
    {
        if (IsReplyComposerOpened)
        {
            var reply = ReplyText!;
            await Coordinator.PublishReplyAsync(Key, reply);
        }
    }

    #endregion

    private async Task DoToggleComments(MouseEventArgs _)
    {
        var expand = false == IsCommentsExpanded;

        if (expand && CommentsCollectionState.Unknown == Comments.State)
        {
            await Coordinator.FetchCommentsAsync(Key);
        }

        IsCommentsExpanded = expand;
    }
}