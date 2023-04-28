using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using StoryBlog.Web.Client.Blog.Components;
using StoryBlog.Web.Client.Blog.Core;
using StoryBlog.Web.Client.Blog.Store.BlogUseCase;
using StoryBlog.Web.Client.Blog.Store.CreateCommentUseCase;

namespace StoryBlog.Web.Client.Blog.Pages;

public partial class Blog
{
    //private readonly Queue<FetchCommentsEventArgs> queue;

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

    private ICommand<Comment> SendReply
    {
        get;
    }

    public Blog()
    {
        //queue = new Queue<FetchCommentsEventArgs>();
        SendReply = new DelegateCommand<Comment>(DoSendReply);
    }

    /*public override Task SetParametersAsync(ParameterView parameters)
    {
        return base.SetParametersAsync(parameters);
    }*/

    protected override void OnInitialized()
    {
        base.OnInitialized();
        
        //CommentsStore.StateChanged += DoCommentsStoreStateChanged;
        Dispatcher.Dispatch(new FetchPostReferenceAction(Slug));
    }

    private void DoSendReply(Comment parameter)
    {
        var action = new CreateCommentAction(
            parameter.PostKey,
            parameter.Key,
            parameter.ReplyText!
        );

        Dispatcher.Dispatch(action);
    }

    private void DoFetchComments(FetchCommentsEventArgs e)
    {
        Dispatcher.Dispatch(new FetchChildCommentsAction(e.PostKey, e.ParentKey));
    }

    /*private void DoCommentsStoreStateChanged(object? sender, EventArgs e)
    {
        var store = CommentsStore.Value;

        if (CommentsKind.Children == store.Kind)
        {
            if (queue.TryPeek(out var eventArgs))
            {
                if (store.PostKey == eventArgs.PostKey && store.ParentKey == eventArgs.ParentKey)
                {
                    eventArgs.Result.SetResult(store.Comments);
                }
                else
                {
                    throw new Exception();
                }
            }
            else
            {
                throw new Exception();
            }
        }
        else
        {
            ;
        }
    }*/
}