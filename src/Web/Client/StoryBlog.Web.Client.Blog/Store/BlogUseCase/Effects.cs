using Fluxor;
using StoryBlog.Web.Client.Blog.Clients.Interfaces;

namespace StoryBlog.Web.Client.Blog.Store.BlogUseCase;

/// <summary>
/// 
/// </summary>
public sealed class FetchPostReferenceActionEffect : Effect<FetchPostReferenceAction>
{
    private readonly ISlugClient client;

    public FetchPostReferenceActionEffect(ISlugClient client)
    {
        this.client = client;
    }

    public override async Task HandleAsync(FetchPostReferenceAction action, IDispatcher dispatcher)
    {
        var postReference = await client.FetchPostReferenceAsync(action.Slug);

        if (null != postReference)
        {
            var fetchPostAction = new FetchPostAction(postReference.Key);
            
            dispatcher.Dispatch(fetchPostAction);
            
            return;
        }

        dispatcher.Dispatch(new FetchPostFailedAction());
    }
}

/// <summary>
/// 
/// </summary>
public sealed class FetchPostEffect : Effect<FetchPostAction>
{
    private readonly IPostsClient client;

    public FetchPostEffect(IPostsClient client)
    {
        this.client = client;
    }

    public override async Task HandleAsync(FetchPostAction action, IDispatcher dispatcher)
    {
        var post = await client.GetPostAsync(action.Key);

        if (null != post)
        {
            dispatcher.Dispatch(new FetchPostReadyAction(action.Key, post));
            return;
        }

        dispatcher.Dispatch(new FetchPostFailedAction());
    }
}

/// <summary>
/// 
/// </summary>
public sealed class FetchCommentsEffect : Effect<FetchPostReadyAction>
{
    private readonly ICommentsClient client;

    public FetchCommentsEffect(ICommentsClient client)
    {
        this.client = client;
    }

    public override async Task HandleAsync(FetchPostReadyAction action, IDispatcher dispatcher)
    {
        var comments = await client.GetCommentsAsync(action.Key);

        if (null != comments)
        {
            var commentsReadyAction = new FetchCommentsReadyAction(action.Key, comments.Comments);

            dispatcher.Dispatch(commentsReadyAction);

            return;
        }

        dispatcher.Dispatch(new FetchCommentsFailedAction());
    }
}