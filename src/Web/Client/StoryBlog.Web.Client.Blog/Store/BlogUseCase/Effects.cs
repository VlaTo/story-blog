using Fluxor;
using StoryBlog.Web.Client.Blog.Clients.Interfaces;

namespace StoryBlog.Web.Client.Blog.Store.BlogUseCase;

#region Blog action effects

/// <summary>
/// 
/// </summary>
// ReSharper disable once UnusedMember.Global
public sealed class FetchPostReferenceActionEffect : Effect<FetchPostReferenceAction>
{
    private readonly ISlugClient client;

    public FetchPostReferenceActionEffect(ISlugClient client)
    {
        this.client = client;
    }

    public override async Task HandleAsync(FetchPostReferenceAction action, IDispatcher dispatcher)
    {
        try
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
        catch
        {
            dispatcher.Dispatch(new FetchPostFailedAction());
        }
    }
}

/// <summary>
/// 
/// </summary>
// ReSharper disable once UnusedMember.Global
public sealed class FetchPostEffect : Effect<FetchPostAction>
{
    private readonly IPostsClient client;

    public FetchPostEffect(IPostsClient client)
    {
        this.client = client;
    }

    public override async Task HandleAsync(FetchPostAction action, IDispatcher dispatcher)
    {
        try
        {
            var post = await client.GetPostAsync(action.Key);

            if (null != post)
            {
                dispatcher.Dispatch(new FetchPostReadyAction(action.Key, post));
                
                return;
            }

            dispatcher.Dispatch(new FetchPostFailedAction());
        }
        catch
        {
            dispatcher.Dispatch(new FetchPostFailedAction());
        }
    }
}

#endregion

#region Comments action effects

/// <summary>
/// 
/// </summary>
// ReSharper disable once UnusedMember.Global
public sealed class FetchPostReadyEffect : Effect<FetchPostReadyAction>
{
    public override Task HandleAsync(FetchPostReadyAction action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new FetchRootCommentsAction(action.Key, 1, 30));
        return Task.CompletedTask;
    }
}

/// <summary>
/// 
/// </summary>
// ReSharper disable once UnusedMember.Global
public sealed class FetchRootCommentsEffect : Effect<FetchRootCommentsAction>
{
    private readonly ICommentsClient client;

    public FetchRootCommentsEffect(ICommentsClient client)
    {
        this.client = client;
    }

    public override async Task HandleAsync(FetchRootCommentsAction action, IDispatcher dispatcher)
    {
        try
        {
            var comments = await client.GetRootCommentsAsync(action.PostKey, action.PageNumber, action.PageSize);

            if (null != comments)
            {
                var commentsReadyAction = new FetchRootCommentsReadyAction(action.PostKey, action.PageNumber, action.PageSize, comments.Comments);

                dispatcher.Dispatch(commentsReadyAction);

                return;
            }

            dispatcher.Dispatch(new FetchCommentsFailedAction(action.PostKey, null));
        }
        catch
        {
            dispatcher.Dispatch(new FetchCommentsFailedAction(action.PostKey, null));
        }
    }
}

/// <summary>
/// 
/// </summary>
// ReSharper disable once UnusedMember.Global
public sealed class FetchChildCommentsEffect : Effect<FetchChildCommentsAction>
{
    private readonly ICommentsClient client;

    public FetchChildCommentsEffect(ICommentsClient client)
    {
        this.client = client;
    }

    public override async Task HandleAsync(FetchChildCommentsAction action, IDispatcher dispatcher)
    {
        try
        {
            var comments = await client.GetChildCommentsAsync(action.PostKey, action.ParentKey);

            if (null != comments)
            {
                var commentsReadyAction = new FetchChildCommentsReadyAction(action.PostKey, action.ParentKey, comments);

                dispatcher.Dispatch(commentsReadyAction);

                return;
            }

            dispatcher.Dispatch(new FetchCommentsFailedAction(action.PostKey, action.ParentKey));
        }
        catch
        {
            dispatcher.Dispatch(new FetchCommentsFailedAction(action.PostKey, action.ParentKey));
        }
    }
}

#endregion