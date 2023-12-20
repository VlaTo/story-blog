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
        new(state.PageNumber, state.PageSize, state.PagesCount, state.Posts, StoreState.Loading);
}

/// <summary>
/// 
/// </summary>
internal sealed class FetchPostsPageFailedReducer : Reducer<HomeState, FetchPostsPageFailedAction>
{
    public override HomeState Reduce(HomeState state, FetchPostsPageFailedAction action) =>
        new(state.PageNumber, state.PageSize, state.PagesCount, state.Posts, StoreState.Failed);
}

/// <summary>
/// 
/// </summary>
internal sealed class FetchPostsPageReadyReducer : Reducer<HomeState, FetchPostsPageReadyAction>
{
    public override HomeState Reduce(HomeState state, FetchPostsPageReadyAction action)
    {
        var posts = action.Posts
            .Select(x => x.ToModel(PostState.Active))
            .ToArray();
        return new(action.PageNumber, action.PageSize, action.PagesCount, posts, StoreState.Success);
    }
}

/// <summary>
/// 
/// </summary>
internal sealed class PermanentPostDeleteReducer : Reducer<HomeState, PermanentPostDeleteAction>
{
    public override HomeState Reduce(HomeState state, PermanentPostDeleteAction action)
    {
        var posts = state.Posts
            .MapReduce(x => x.Key == action.PostKey, x => x with
            {
                State = PostState.Updating
            })
            .ToArray();
        return new HomeState(state.PageNumber, state.PageSize, state.PagesCount, posts, state.StoreState);
    }
}

/// <summary>
/// 
/// </summary>
internal sealed class PermanentPostDeleteSuccessReducer : Reducer<HomeState, PermanentPostDeleteSuccessAction>
{
    public override HomeState Reduce(HomeState state, PermanentPostDeleteSuccessAction action)
    {
        var posts = state.Posts
            .Exclude(x => x.Key == action.PostKey)
            .ToArray();
        return new HomeState(state.PageNumber, state.PageSize, state.PagesCount, posts, state.StoreState);
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
        return new HomeState(state.PageNumber, state.PageSize, state.PagesCount, posts, state.StoreState);
    }
}

/// <summary>
/// 
/// </summary>
internal sealed class ChangePostVisibilityReducer : Reducer<HomeState, ChangePostVisibilityAction>
{
    public override HomeState Reduce(HomeState state, ChangePostVisibilityAction action)
    {
        var posts = state.Posts
            .MapReduce(x => x.Key == action.PostKey, x => x with
            {
                State = PostState.Updating
            })
            .ToArray();
        return new HomeState(state.PageNumber, state.PageSize, state.PagesCount, posts, state.StoreState);
    }
}

/// <summary>
/// 
/// </summary>
internal sealed class ChangePostVisibilitySuccessReducer : Reducer<HomeState, ChangePostVisibilitySuccessAction>
{
    public override HomeState Reduce(HomeState state, ChangePostVisibilitySuccessAction action)
    {
        var posts = state.Posts
            .MapReduce(x => x.Key == action.PostKey, x => x with
            {
                State = PostState.Active,
                VisibilityStatus = action.VisibilityStatus
            })
            .ToArray();
        return new HomeState(state.PageNumber, state.PageSize, state.PagesCount, posts, state.StoreState);
    }
}