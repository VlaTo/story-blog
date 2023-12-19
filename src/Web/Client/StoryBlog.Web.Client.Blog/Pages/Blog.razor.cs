using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using StoryBlog.Web.Client.Blog.Components;
using StoryBlog.Web.Client.Blog.Models;
using StoryBlog.Web.Client.Blog.Store.BlogUseCase;

namespace StoryBlog.Web.Client.Blog.Pages;

public partial class Blog : ICommentsObserver
{
    private bool isReplyComposerOpened;
    private string? correlationKey;
    private Comments? comments;
    private IDisposable? subscription;

    [Parameter]
    public string SlugOrKey
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

    [Inject]
    public IState<BlogState> PostStore
    {
        get;
        set;
    }

    [Inject]
    public IState<CommentsState> CommentsStore
    {
        get;
        set;
    }

    [Inject]
    private IDispatcher Dispatcher
    {
        get;
        set;
    }

    [Inject]
    private IStringLocalizer<Blog> Localizer
    {
        get;
        set;
    }

    private string? ReplyText
    {
        get;
        set;
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Dispatcher.Dispatch(new FetchPostAction(SlugOrKey));
    }

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        if (null != comments && null == subscription)
        {
            subscription = comments.Subscribe(this);
        }
    }

    private void UpdateReplyStatus(CommentsState? store)
    {
        if (false == IsReplyComposerOpened)
        {
            return;
        }

        if (String.IsNullOrEmpty(correlationKey))
        {
            return;
        }

        if (null != store?.Comments)
        {
            var comments = store.Comments;

            for (var index = 0; index < comments.Count; index++)
            {
                if (comments[index] is CommentReplyModel reply)
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
                else if (comments[index] is NewCommentModel newComment)
                {
                    if (newComment.CorrelationKey == correlationKey)
                    {
                        correlationKey = null;
                        isReplyComposerOpened = false;
                    }
                }
            }
        }
    }

    private void DoOpenReplyComposer(MouseEventArgs _)
    {
        if (false == IsReplyComposerOpened)
        {
            comments?.NotifyReplyComposerOpened(this);
        }

        IsReplyComposerOpened = false == IsReplyComposerOpened;
    }

    private void DoSendReply(MouseEventArgs _)
    {
        if (IsReplyComposerOpened && false == String.IsNullOrEmpty(ReplyText))
        {
            correlationKey = Guid.NewGuid().ToString("N");
            DoPublishReply(new PublishReplyEventArgs(null, ReplyText, correlationKey));
        }
    }

    private void DoCancelReplyComposer(MouseEventArgs _)
    {
        if (IsReplyComposerOpened)
        {
            correlationKey = null;
            IsReplyComposerOpened = false;

            StateHasChanged();
        }
    }

    private void DoFetchComments(FetchCommentsEventArgs e)
    {
        var postKey = PostStore.Value.Post!.Key;
        var action = new FetchChildCommentsAction(postKey, e.ParentKey);

        Dispatcher.Dispatch(action);
    }

    private void DoPublishReply(PublishReplyEventArgs e)
    {
        var postKey = PostStore.Value.Post!.Key;
        var action = new PublishReplyAction(postKey, e.ParentKey, e.CorrelationKey, e.Reply);
        
        Dispatcher.Dispatch(action);
    }

    void ICommentsObserver.OnOpenReplyComposer()
    {
        DoCancelReplyComposer(new MouseEventArgs());
    }
}