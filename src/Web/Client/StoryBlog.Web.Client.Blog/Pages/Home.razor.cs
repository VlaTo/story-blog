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
    private const int PageSize = 3;

    private int selectedPageNumber;

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

    private int SelectedPageNumber
    {
        get => selectedPageNumber;
        set
        {
            if (selectedPageNumber == value)
            {
                return ;
            }

            selectedPageNumber = value;
            Dispatcher.Dispatch(new FetchPostsPageAction(value, PageSize));
        }
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

        selectedPageNumber = 1;
        Dispatcher.Dispatch(new FetchPostsPageAction(selectedPageNumber, PageSize));
    }

    protected override async Task OnInitializedAsync()
    {
        var authenticationState = await AuthenticationStateTask;
        CurrentUser = authenticationState.User;
    }

    private bool IsAuthorOf(BriefPostModel post)
    {
        if (CurrentUser?.IsAuthenticated() ?? false)
        {
            var subject = CurrentUser.GetSubject();
            return String.Equals(post.Author, subject);
        }

        return false;
    }

    private bool IsTogglePublicDisabled(BriefPostModel post) =>
        false == HasPermission(post, Permissions.Blogs.Update, AllowedActions.CanTogglePublic);

    private bool IsEditDisabled(BriefPostModel post) =>
        false == HasPermission(post, Permissions.Blogs.Update, AllowedActions.CanEdit);

    private bool IsDeleteDisabled(BriefPostModel post) =>
        false == HasPermission(post, Permissions.Blogs.Delete, AllowedActions.CanDelete);

    private bool HasPermission(BriefPostModel post, string permission, AllowedActions action)
    {
        if (CurrentUser?.IsAuthenticated() ?? false)
        {
            var subject = CurrentUser.GetSubject();

            if (String.Equals(post.Author, subject))
            {
                var hasPermission = CurrentUser.HasPermission(permission);
                return hasPermission && action == (post.AllowedActions & action);
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
            var store = Store.Value;
            var lastPost = store.Posts.Last(x => x.Key != postKey);
            var action = new ImmediatePostDeleteAction(postKey, lastPost.Key, store.PageNumber, store.PageSize);

            Dispatcher.Dispatch(action);
        }
    }

    private void DoPostEdit(string slug)
    {
        NavigationManager.NavigateTo($"edit/{slug}");
    }

    private void DoTogglePublic(Guid postKey, bool isPublic)
    {
        var action = new TogglePostPublicityAction(postKey, isPublic);
        Dispatcher.Dispatch(action);
    }
}
