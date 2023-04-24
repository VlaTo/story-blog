using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using System.Windows.Input;
using Fluxor;
using Microsoft.AspNetCore.Components;
using StoryBlog.Web.Client.Blog.Components;
using StoryBlog.Web.Client.Blog.Core;
using StoryBlog.Web.Client.Blog.Store.BlogUseCase;
using StoryBlog.Web.Client.Blog.Store.CreateCommentUseCase;

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
    public IState<BlogState> Store
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
    private ICommentEditorCoordinator EditorCoordinator
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
        SendReply = new DelegateCommand<Comment>(DoSendReply);
    }

    public override Task SetParametersAsync(ParameterView parameters)
    {
        return base.SetParametersAsync(parameters);
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Dispatcher.Dispatch(new FetchPostReferenceAction(Slug));
    }

    private void DoSendReply(Comment parameter)
    {
        var action = new CreateCommentAction(
            parameter.PostKey,
            parameter.ParentKey,
            parameter.CommentReply!,
            DateTime.UtcNow
        );

        Dispatcher.Dispatch(action);
    }
}