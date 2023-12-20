using Fluxor;
using StoryBlog.Web.Client.Blog.Clients;

namespace StoryBlog.Web.Client.Blog.Store.CreatePostUseCase;

/// <summary>
/// 
/// </summary>
internal sealed class GenerateSlugEffect : Effect<GenerateSlugAction>
{
    private readonly PostsHttpClient client;

    public GenerateSlugEffect(PostsHttpClient client)
    {
        this.client = client;
    }

    public override async Task HandleAsync(GenerateSlugAction action, IDispatcher dispatcher)
    {
        var result = await client.GenerateSlugAsync(action.Title);

        object next = null != result
            ? new GeneratedSlugReadyAction(result.Title, result.Slug)
            : new GenerateSlugFailedAction(action.Title);

        dispatcher.Dispatch(next);
    }
}

/// <summary>
/// 
/// </summary>
internal sealed class CreatePostEffect : Effect<CreatePostAction>
{
    private readonly PostsHttpClient client;

    public CreatePostEffect(PostsHttpClient client)
    {
        this.client = client;
    }

    public override async Task HandleAsync(CreatePostAction action, IDispatcher dispatcher)
    {
        var result = await client.CreatePostAsync(action.Title, action.Slug, action.Content);

        object next = null != result
            ? new CreatedPostReadyAction(result.Title, result.Slug, result.Status, result.CreatedAt)
            : new CreatePostFailedAction();

        dispatcher.Dispatch(next);
    }
}