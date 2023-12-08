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
using StoryBlog.Web.Client.Blog.Extensions;
using StoryBlog.Web.Client.Blog.Models;

namespace StoryBlog.Web.Client.Blog.Store.HomeUseCase;

/// <summary>
/// 
/// </summary>
internal sealed class FetchPostsPageReducer : Reducer<HomeState, FetchPostsPageAction>
{
    public override HomeState Reduce(HomeState state, FetchPostsPageAction action) =>
        new(state.PageNumber, state.PageSize, state.Posts, StoreState.Loading);
}

/// <summary>
/// 
/// </summary>
internal sealed class FetchPostsPageFailedReducer : Reducer<HomeState, FetchPostsPageFailedAction>
{
    public override HomeState Reduce(HomeState state, FetchPostsPageFailedAction action) =>
        new(state.PageNumber, state.PageSize, state.Posts, StoreState.Failed);
}

/// <summary>
/// 
/// </summary>
internal sealed class PostsPageReadyReducer : Reducer<HomeState, FetchPostsPageReadyAction>
{
    public override HomeState Reduce(HomeState state, FetchPostsPageReadyAction action)
    {
        var posts = action.Posts
            .Select(x => x.ToModel(PostState.Active))
            .ToArray();
        return new(state.PageNumber, state.PageSize, posts, StoreState.Success);
    }
}

/// <summary>
/// 
/// </summary>
internal sealed class ImmediatePostDeleteReducer : Reducer<HomeState, ImmediatePostDeleteAction>
{
    public override HomeState Reduce(HomeState state, ImmediatePostDeleteAction action)
    {
        var posts = state.Posts
            .MapReduce(x => x.Key == action.PostKey, x => x with
            {
                State = PostState.Deleting
            })
            .ToArray();
        return new HomeState(state.PageNumber, state.PageSize, posts, state.StoreState);
    }
}

/// <summary>
/// 
/// </summary>
internal sealed class ImmediatePostDeleteSuccessReducer : Reducer<HomeState, ImmediatePostDeleteSuccessAction>
{
    public override HomeState Reduce(HomeState state, ImmediatePostDeleteSuccessAction action)
    {
        var posts = state.Posts
            .Exclude(x => x.Key == action.PostKey)
            .ToArray();
        return new HomeState(state.PageNumber, state.PageSize, posts, state.StoreState);
    }
}

/// <summary>
/// 
/// </summary>
internal sealed class TailPostsReadyReducer : Reducer<HomeState, TailPostsReadyAction>
{
    public override HomeState Reduce(HomeState state, TailPostsReadyAction action)
    {
        var posts = new List<BriefPostModel>(state.Posts);
        posts.AddRange(action.Posts.Select(x => x.ToModel(PostState.Active)));
        return new HomeState(state.PageNumber, state.PageSize, posts, state.StoreState);
    }
}