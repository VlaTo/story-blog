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
using StoryBlog.Web.Client.Blog.Clients.Interfaces;
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
            empty => new FetchPostsPageReadyAction(Array.Empty<BriefModel>(), action.PageNumber, action.PageSize),
            result => new FetchPostsPageReadyAction(result.Posts, result.PageNumber, result.PageSize),
            exception => new FetchPostsPageFailedAction(action.PageNumber, action.PageSize)
        );

        dispatcher.Dispatch(next);
    }
}