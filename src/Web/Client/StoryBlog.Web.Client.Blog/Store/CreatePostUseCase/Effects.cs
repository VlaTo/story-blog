using Fluxor;
using StoryBlog.Web.Client.Blog.Clients.Interfaces;

namespace StoryBlog.Web.Client.Blog.Store.CreatePostUseCase;

/// <summary>
/// 
/// </summary>
public sealed class GenerateSlugEffect : Effect<GenerateSlugAction>
{
    private readonly ISlugClient client;

    public GenerateSlugEffect(ISlugClient client)
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
public sealed class CreatePostEffect : Effect<CreatePostAction>
{
    private readonly IPostsClient client;

    public CreatePostEffect(IPostsClient client)
    {
        this.client = client;
    }

    public override async Task HandleAsync(CreatePostAction action, IDispatcher dispatcher)
    {
        var result = await client.CreatePostAsync(action.Title, action.Slug);

        object next = null != result
            ? new CreatedPostReadyAction(result.Title, result.Slug, result.Status, result.CreatedAt)
            : new CreatePostFailedAction();

        dispatcher.Dispatch(next);
    }
}