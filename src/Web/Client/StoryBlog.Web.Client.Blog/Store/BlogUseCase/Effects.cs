using Fluxor;
using Microsoft.AspNetCore.Components.Authorization;
using StoryBlog.Web.Client.Blog.Clients.Interfaces;

namespace StoryBlog.Web.Client.Blog.Store.BlogUseCase;

#region Blog action effects

/// <summary>
/// 
/// </summary>
// ReSharper disable once UnusedMember.Global
public sealed class FetchPostEffect : Effect<FetchPostAction>
{
    private readonly IPostsClient client;
    private readonly AuthenticationStateProvider authenticationProvider;

    public FetchPostEffect(IPostsClient client, AuthenticationStateProvider authenticationProvider)
    {
        this.client = client;
        this.authenticationProvider = authenticationProvider;
    }

    public override async Task HandleAsync(FetchPostAction action, IDispatcher dispatcher)
    {
         await authenticationProvider.GetAuthenticationStateAsync();

        try
        {
            var post = await client.GetPostAsync(action.SlugOrKey);

            if (null != post)
            {
                dispatcher.Dispatch(new FetchPostReadyAction(action.SlugOrKey, post));
                
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
        var postKey = action.Post.Key;
        
        dispatcher.Dispatch(new FetchRootCommentsAction(postKey, 1, 30));
        
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

/// <summary>
/// 
/// </summary>
// ReSharper disable once UnusedMember.Global
public sealed class PublishReplyEffect : Effect<PublishReplyAction>
{
    private readonly ICommentsClient client;

    public PublishReplyEffect(ICommentsClient client)
    {
        this.client = client;
    }

    public override async Task HandleAsync(PublishReplyAction action, IDispatcher dispatcher)
    {
        try
        {
            var comment = await client.CreateCommentAsync(action.PostKey, action.ParentKey, action.Reply);

            if (null != comment)
            {
                var commentsReadyAction = new PublishReplySuccessAction(comment, action.CorrelationId);

                dispatcher.Dispatch(commentsReadyAction);

                return;
            }

            dispatcher.Dispatch(new PublishReplyFailedAction(action.PostKey, action.ParentKey, action.CorrelationId));
        }
        catch
        {
            dispatcher.Dispatch(new PublishReplyFailedAction(action.PostKey, action.ParentKey, action.CorrelationId));
        }
    }
}

#endregion