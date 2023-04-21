using Fluxor;
using Microsoft.AspNetCore.Components;
using StoryBlog.Web.Client.Blog.Core;
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

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Dispatcher.Dispatch(new FetchPostReferenceAction(Slug));
    }

    public override Task SetParametersAsync(ParameterView parameters)
    {
        return base.SetParametersAsync(parameters);
    }
}