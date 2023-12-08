using Fluxor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using StoryBlog.Web.Client.Blog.Extensions;
using StoryBlog.Web.Client.Blog.Store.HomeUseCase;
using StoryBlog.Web.Common.Identity.Permission;
using StoryBlog.Web.Microservices.Posts.Shared.Models;
using System.Security.Claims;
using StoryBlog.Web.Client.Blog.Models;
using StoryBlog.Web.Client.Blog.Store;

namespace StoryBlog.Web.Client.Blog.Pages;

[AllowAnonymous]
public partial class Home
{
    [Inject]
    private IState<HomeState> Store
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
    private NavigationManager NavigationManager
    {
        get;
        set;
    }

    [Inject]
    private IDialogService DialogService
    {
        get;
        set;
    }

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask
    {
        get;
        set;
    }

    private bool IsLoading => StoreState.Loading == Store.Value.StoreState;

    private ClaimsPrincipal? CurrentUser
    {
        get;
        set;
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        Dispatcher.Dispatch(new FetchPostsPageAction(1, 3));
    }

    protected override async Task OnInitializedAsync()
    {
        //return base.OnInitializedAsync();
        var authenticationState = await AuthenticationStateTask;
        CurrentUser = authenticationState.User;
    }

    private bool CanEdit(BriefPostModel post) => HasPermission(post, Permissions.Blogs.Update);

    private bool CanDelete(BriefPostModel post) => HasPermission(post, Permissions.Blogs.Delete);

    private bool HasPermission(BriefPostModel post, string permission)
    {
        if (CurrentUser?.IsAuthenticated() ?? false)
        {
            var subject = CurrentUser.GetSubject();

            if (String.Equals(post.Author, subject))
            {
                var hasPermission = CurrentUser.HasPermission(permission);
                var allowed = AllowedActions.CanEdit == (post.AllowedActions & AllowedActions.CanEdit);
                
                return hasPermission && allowed;
            }
        }

        return false;
    }

    private async void DoPostDelete(Guid postKey, string postTitle)
    {
        var dialog = await DialogService.ShowAsync<DeleteRequestDialog>(
            "Подтверждение",
            new DialogParameters<DeleteRequestDialog>
            {
                {x=>x.PostTitle, postTitle}
            },
            new DialogOptions
            {
                Position = DialogPosition.TopCenter,
                CloseOnEscapeKey = true
            }
        );

        var result = await dialog.GetReturnValueAsync<bool?>();

        if (result.HasValue && result.Value)
        {
            var lastPost = Store.Value.Posts
                .Last(x => x.Key != postKey);

            var action = new ImmediatePostDeleteAction(postKey, lastPost.Key, Store.Value.PageNumber, Store.Value.PageSize);
            Dispatcher.Dispatch(action);
        }
    }

    private void DoPostEdit(string slug)
    {
        NavigationManager.NavigateTo($"edit/{slug}");
    }
}
