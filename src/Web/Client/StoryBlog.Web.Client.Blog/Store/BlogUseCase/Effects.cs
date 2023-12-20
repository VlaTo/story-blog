using Fluxor;
using Microsoft.AspNetCore.Components.Authorization;
using StoryBlog.Web.Client.Blog.Clients;
using StoryBlog.Web.Client.Blog.Extensions;

namespace StoryBlog.Web.Client.Blog.Store.BlogUseCase;

#region Blog action effects

/// <summary>
/// 
/// </summary>
// ReSharper disable once UnusedMember.Global
internal sealed class FetchPostEffect : Effect<FetchPostAction>
{
    private readonly PostsHttpClient client;
    private readonly AuthenticationStateProvider authenticationProvider;

    public FetchPostEffect(
        PostsHttpClient client,
        AuthenticationStateProvider authenticationProvider)
    {
        this.client = client;
        this.authenticationProvider = authenticationProvider;
    }

    public override async Task HandleAsync(FetchPostAction action, IDispatcher dispatcher)
    {
        var authenticationState = await authenticationProvider.GetAuthenticationStateAsync();

        Console.WriteLine($"Is User authenticated: {authenticationState.User.IsAuthenticated()}");

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
internal sealed class FetchRootCommentsEffect : Effect<FetchRootCommentsAction>
{
    private readonly CommentsHttpClient client;

    public FetchRootCommentsEffect(CommentsHttpClient client)
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
internal sealed class FetchChildCommentsEffect : Effect<FetchChildCommentsAction>
{
    private readonly CommentsHttpClient client;

    public FetchChildCommentsEffect(CommentsHttpClient client)
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
internal sealed class PublishReplyEffect : Effect<PublishReplyAction>
{
    private readonly CommentsHttpClient client;

    public PublishReplyEffect(CommentsHttpClient client)
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