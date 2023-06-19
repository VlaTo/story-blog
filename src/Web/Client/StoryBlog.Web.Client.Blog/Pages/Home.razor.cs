using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using StoryBlog.Web.Client.Blog.Extensions;
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

    [CascadingParameter]
    private Task<AuthenticationState> authenticationState
    {
        get; 
        set;
    }

    protected override async Task OnInitializedAsync()
    {
        var authentication = await authenticationState;

        Dispatcher.Dispatch(new FetchPostsPageAction(1, 10));

        if (authentication.User.IsAuthenticated())
        {
            ;
        }
    }
}
