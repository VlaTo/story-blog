using Fluxor;

namespace StoryBlog.Web.Client.Blog.Store.BlogUseCase;

public sealed class FetchPostReducer : Reducer<BlogState, FetchPostAction>
{
    public override BlogState Reduce(BlogState state, FetchPostAction action)
    {
        return new BlogState(StoreState.Loading, null, null);
    }
}

public sealed class FetchPostReadyReducer : Reducer<BlogState, FetchPostReadyAction>
{
    public override BlogState Reduce(BlogState state, FetchPostReadyAction action)
    {
        return new BlogState(StoreState.Loading, action.Post, null);
    }
}

public sealed class FetchCommentsReadyReducer : Reducer<BlogState, FetchCommentsReadyAction>
{
    public override BlogState Reduce(BlogState state, FetchCommentsReadyAction action)
    {
        return new BlogState(StoreState.Success, state.Post, action.Comments);
    }
}