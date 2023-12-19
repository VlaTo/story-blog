﻿#region MIT Licence
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
using StoryBlog.Web.Client.Blog.Clients.Interfaces;
using StoryBlog.Web.Common.Result.Extensions;
using StoryBlog.Web.Microservices.Posts.Shared.Models;

namespace StoryBlog.Web.Client.Blog.Store.HomeUseCase;

/// <summary>
/// 
/// </summary>
public sealed class FetchAvailablePostsEffect : Effect<FetchPostsPageAction>
{
    private readonly AuthenticationStateProvider authenticationProvider;
    private readonly IPostsClient client;

    public FetchAvailablePostsEffect(
        AuthenticationStateProvider authenticationProvider,
        IPostsClient client)
    {
        this.authenticationProvider = authenticationProvider;
        this.client = client;
    }

    public override async Task HandleAsync(FetchPostsPageAction action, IDispatcher dispatcher)
    {
        await authenticationProvider.GetAuthenticationStateAsync();

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
public sealed class ImmediatePostDeleteEffect : Effect<ImmediatePostDeleteAction>
{
    private readonly IPostsClient client;

    public ImmediatePostDeleteEffect(IPostsClient client)
    {
        this.client = client;
    }

    public override async Task HandleAsync(ImmediatePostDeleteAction action, IDispatcher dispatcher)
    {
        var slugOrKey = action.PostKey.ToString();
        var response = await client.DeletePostAsync(slugOrKey);

        object nextAction = response.Succeeded
            ? new ImmediatePostDeleteSuccessAction(action.PostKey, action.LastPostKey, action.PageNumber, action.PageSize)
            : new ImmediatePostDeleteFailedAction(action.PostKey, action.PageNumber, action.PageSize);

        dispatcher.Dispatch(nextAction);
    }
}

/// <summary>
/// 
/// </summary>
public sealed class ImmediatePostDeleteSuccessEffect : Effect<ImmediatePostDeleteSuccessAction>
{
    private readonly IPostsClient client;

    public ImmediatePostDeleteSuccessEffect(IPostsClient client)
    {
        this.client = client;
    }

    public override async Task HandleAsync(ImmediatePostDeleteSuccessAction action, IDispatcher dispatcher)
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
public sealed class TogglePostPublicityActionEffect : Effect<TogglePostPublicityAction>
{
    private readonly IPostsClient client;

    public TogglePostPublicityActionEffect(IPostsClient client)
    {
        this.client = client;
    }

    public override async Task HandleAsync(TogglePostPublicityAction action, IDispatcher dispatcher)
    {
        var response = await client.UpdatePostPublicityAsync(action.PostKey, action.IsPublic);

        if (false == response.Succeeded)
        {
            throw response.Error!;
        }

        dispatcher.Dispatch(
            new TogglePublicitySuccessAction(action.PostKey, action.IsPublic)
        );
    }
}