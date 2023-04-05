using System.ComponentModel.DataAnnotations;
using Fluxor;

namespace StoryBlog.Web.Client.Blog.Store.CreatePostUseCase;

[FeatureState]
public sealed class CreatePostState : AbstractStore
{
    public string Title
    {
        get;
    }

    public string Slug
    {
        get;
    }

    public CreatePostState(StoreState storeState, string title, string slug)
        : base(storeState)
    {
        Title = title;
        Slug = slug;
    }

    public CreatePostState()
        : base(StoreState.Empty)
    {
    }
}