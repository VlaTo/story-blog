using Fluxor;
using Microsoft.AspNetCore.Components;
using StoryBlog.Web.Client.Blog.Store.HomeUseCase;

namespace StoryBlog.Web.Client.Blog.Pages;

public partial class Home
{
    [Inject]
    private IState<HomeState> Store
    {
        get; 
        set;
    }

    [Inject]
    public IDispatcher Dispatcher
    {
        get; 
        set;
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        Dispatcher.Dispatch(new FetchPostsPageAction(1, 10));
    }
}
