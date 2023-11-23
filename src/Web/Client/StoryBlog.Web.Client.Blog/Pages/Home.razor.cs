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

    private ClaimsPrincipal? CurrentUser
    {
        get;
        set;
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        Dispatcher.Dispatch(new FetchPostsPageAction(1, 10));
    }

    protected override async Task OnInitializedAsync()
    {
        //return base.OnInitializedAsync();
        var authenticationState = await AuthenticationStateTask;
        CurrentUser = authenticationState.User;
    }

    private bool CanEdit(BriefModel post) => HasPermission(post, Permissions.Blogs.Update);

    private bool CanDelete(BriefModel post) => HasPermission(post, Permissions.Blogs.Delete);

    private bool HasPermission(BriefModel post, string permission)
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
            var action = new ImmediatePostDelete(postKey, Store.Value.PageNumber, Store.Value.PageSize);
            Dispatcher.Dispatch(action);
        }
    }

    private void DoPostEdit(string slug)
    {
        NavigationManager.NavigateTo($"edit/{slug}");
    }
}
