using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using StoryBlog.Web.Client.Blog.Components;
using StoryBlog.Web.Client.Blog.Store.BlogUseCase;

namespace StoryBlog.Web.Client.Blog.Pages;

public partial class Blog
{
    [Parameter]
    public string Slug
    {
        get;
        set;
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

    protected override void OnInitialized()
    {
        base.OnInitialized();
        
        Dispatcher.Dispatch(new FetchPostReferenceAction(Slug));
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
        var action = new PublishReplyAction(postKey, e.ParentKey, e.CorrelationId, e.Reply);
        
        Dispatcher.Dispatch(action);
    }
}