using Fluxor;

namespace StoryBlog.Web.Client.Blog.Store.CreatePostUseCase;

/// <summary>
/// 
/// </summary>
public sealed class GenerateSlugReducer : Reducer<SlugState, GenerateSlugAction>
{
    public override SlugState Reduce(SlugState state, GenerateSlugAction action)
    {
        return new SlugState(StoreState.Loading, state.Slug);
    }
}

/// <summary>
/// 
/// </summary>
public sealed class GeneratedSlugReadyReducer : Reducer<SlugState, GeneratedSlugReadyAction>
{
    public override SlugState Reduce(SlugState state, GeneratedSlugReadyAction action)
    {
        return new SlugState(StoreState.Success, action.Slug);
    }
}

/// <summary>
/// 
/// </summary>
public sealed class GenerateSlugFailedReducer : Reducer<SlugState, GenerateSlugFailedAction>
{
    public override SlugState Reduce(SlugState state, GenerateSlugFailedAction action)
    {
        return new SlugState(StoreState.Failed, state.Slug);
    }
}

/// <summary>
/// 
/// </summary>
public sealed class CreatePostReducer : Reducer<CreatePostState, CreatePostAction>
{
    public override CreatePostState Reduce(CreatePostState state, CreatePostAction action)
    {
        return new CreatePostState(StoreState.Loading, state.Title, state.Slug);
    }
}

/// <summary>
/// 
/// </summary>
public sealed class CreatedPostReadyReducer : Reducer<CreatePostState, CreatedPostReadyAction>
{
    public override CreatePostState Reduce(CreatePostState state, CreatedPostReadyAction action)
    {
        return new CreatePostState(StoreState.Success, state.Title, state.Slug);
    }
}

/// <summary>
/// 
/// </summary>
public sealed class CreatePostFailedReducer : Reducer<CreatePostState, CreatePostFailedAction>
{
    public override CreatePostState Reduce(CreatePostState state, CreatePostFailedAction action)
    {
        return new CreatePostState(StoreState.Failed, state.Title, state.Slug);
    }
}