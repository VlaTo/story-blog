using Fluxor;

namespace StoryBlog.Web.Client.Blog.Store.CreatePostUseCase;

[FeatureState]
public sealed class SlugState : AbstractStore
{
    public string? Slug
    {
        get;
    } 

    public SlugState()
        : base(StoreState.Empty)
    {
    }

    public SlugState(StoreState storeState, string? slug)
        : base(storeState)
    {
        Slug = slug;
    }
}