#region MIT Licence
// Copyright 2023 Vladimir Tolmachev
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the “Software”), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies
// or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
#endregion

using Fluxor;
using Microsoft.AspNetCore.Components.Authorization;
using StoryBlog.Web.Client.Blog.Clients;
using StoryBlog.Web.Client.Blog.Extensions;
using StoryBlog.Web.Common.Result.Extensions;
using StoryBlog.Web.Microservices.Posts.Shared.Models;

namespace StoryBlog.Web.Client.Blog.Store.HomeUseCase;

/// <summary>
/// 
/// </summary>
internal sealed class FetchAvailablePostsEffect : Effect<FetchPostsPageAction>
{
    private readonly AuthenticationStateProvider authenticationProvider;
    private readonly PostsHttpClient client;
    private readonly ILogger<FetchAvailablePostsEffect> logger;

    public FetchAvailablePostsEffect(
        AuthenticationStateProvider authenticationProvider,
        PostsHttpClient client,
        ILogger<FetchAvailablePostsEffect> logger)
    {
        this.authenticationProvider = authenticationProvider;
        this.client = client;
        this.logger = logger;
    }

    public override async Task HandleAsync(FetchPostsPageAction action, IDispatcher dispatcher)
    {
        var authenticationState = await authenticationProvider.GetAuthenticationStateAsync();

        logger.LogDebug($"User authenticated: {authenticationState.User.IsAuthenticated()}");

        var response = await client.GetPostsAsync(action.PageNumber, action.PageSize);

        var next = response.Select<object>(
            empty => new FetchPostsPageReadyAction(Array.Empty<BriefModel>(), action.PageNumber, action.PageSize, 0),
            result => new FetchPostsPageReadyAction(result.Posts, result.PageNumber, result.PageSize, result.PagesCount),
            exception => new FetchPostsPageFailedAction(action.PageNumber, action.PageSize)
        );

        dispatcher.Dispatch(next);
    }
}

/// <summary>
/// 
/// </summary>
internal sealed class ImmediatePostDeleteEffect : Effect<PermanentPostDeleteAction>
{
    private readonly PostsHttpClient client;

    public ImmediatePostDeleteEffect(PostsHttpClient client)
    {
        this.client = client;
    }

    public override async Task HandleAsync(PermanentPostDeleteAction action, IDispatcher dispatcher)
    {
        var slugOrKey = action.PostKey.ToString();
        var response = await client.DeletePostAsync(slugOrKey);

        object nextAction = response.Succeeded
            ? new PermanentPostDeleteSuccessAction(action.PostKey, action.LastPostKey, action.PageNumber, action.PageSize)
            : new PermanentPostDeleteFailedAction(action.PostKey, action.PageNumber, action.PageSize);

        dispatcher.Dispatch(nextAction);
    }
}

/// <summary>
/// 
/// </summary>
internal sealed class ImmediatePostDeleteSuccessEffect : Effect<PermanentPostDeleteSuccessAction>
{
    private readonly PostsHttpClient client;

    public ImmediatePostDeleteSuccessEffect(PostsHttpClient client)
    {
        this.client = client;
    }

    public override async Task HandleAsync(PermanentPostDeleteSuccessAction action, IDispatcher dispatcher)
    {
        var response = await client.GetTailPostsAsync(action.LastPostKey, 1);

        if (response.Failed())
        {
            throw new Exception();
        }

        dispatcher.Dispatch(
            new TailPostsReadyAction(action.LastPostKey, response.Item2!, action.PageNumber, action.PageSize)
        );
    }
}

/// <summary>
/// 
/// </summary>
internal sealed class ChangePostVisibilityActionEffect : Effect<ChangePostVisibilityAction>
{
    private readonly PostsHttpClient client;

    public ChangePostVisibilityActionEffect(PostsHttpClient client)
    {
        this.client = client;
    }

    public override async Task HandleAsync(ChangePostVisibilityAction action, IDispatcher dispatcher)
    {
        var response = await client.ChangePostVisibilityAsync(action.PostKey, action.VisibilityStatus);

        if (false == response.Succeeded)
        {
            throw response.Error!;
        }

        dispatcher.Dispatch(
            new ChangePostVisibilitySuccessAction(action.PostKey, action.VisibilityStatus)
        );
    }
}