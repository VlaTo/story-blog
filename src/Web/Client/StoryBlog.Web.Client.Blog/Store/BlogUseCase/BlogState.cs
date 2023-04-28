using Fluxor;
using StoryBlog.Web.Microservices.Posts.Shared.Models;

namespace StoryBlog.Web.Client.Blog.Store.BlogUseCase;

[FeatureState]
public sealed class BlogState : AbstractStore
{
    public PostModel? Post
    {
        get;
    }

    public BlogState()
        : base(StoreState.Empty)
    {
    }

    public BlogState(StoreState storeState, PostModel? post)
        : base(storeState)
    {
        Post = post;
    }
}