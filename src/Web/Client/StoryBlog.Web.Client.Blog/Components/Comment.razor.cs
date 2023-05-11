using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using MudBlazor.Utilities;
using StoryBlog.Web.Client.Blog.Core;
using StoryBlog.Web.Client.Blog.Models;

namespace StoryBlog.Web.Client.Blog.Components;

/// <summary>
/// 
/// </summary>
public partial class Comment : ICommentsObserver, IDisposable
{
    private IDisposable? editorSubscription;
    private bool isReplyComposerOpened;
    private string? correlationKey;

    protected string Classname => new CssBuilder("storyblog-blog-comment")
        .AddClass("new-comment", IsNew)
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
        get => isReplyComposerOpened;
        set
        {
            if (isReplyComposerOpened == value)
            {
                return;
            }

            isReplyComposerOpened = value;

            StateHasChanged();
        }
    }

    [Parameter]
    public bool IsNew
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

    private void UpdateReplyStatus()
    {
        if (false == IsReplyComposerOpened)
        {
            return ;
        }

        if (String.IsNullOrEmpty(correlationKey))
        {
            return ;
        }

        for (var index = 0; index < Comments.Count; index++)
        {
            if (Comments[index] is CommentReplyModel reply)
            {
                if (reply.CorrelationKey == correlationKey)
                {
                    switch (reply.Result)
                    {
                        case CommentReplyResult.Publishing:
                        {
                            break;
                        }

                        case CommentReplyResult.Failed:
                        {
                            break;
                        }
                    }
                }
            }
            else if (Comments[index] is NewCommentModel newComment)
            {
                if (newComment.CorrelationKey == correlationKey)
                {
                    correlationKey = null;
                    isReplyComposerOpened = false;
                    ReplyText = null;
                }
            }
        }
    }

    #region Reply compose callbacks

    private void DoOpenReplyComposer(MouseEventArgs _)
    {
        if (IsReplyComposerOpened)
        {
            return ;
        }

        IsReplyComposerOpened = true;

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
            if (String.IsNullOrEmpty(ReplyText))
            {
                return ;
            }

            correlationKey = Guid.NewGuid().ToString("N");

            await Coordinator.PublishReplyAsync(Key, ReplyText, correlationKey);
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