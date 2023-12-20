using Fluxor;
using StoryBlog.Web.Client.Blog.Core;
using StoryBlog.Web.Client.Blog.Extensions;
using StoryBlog.Web.Client.Blog.Models;

namespace StoryBlog.Web.Client.Blog.Store.BlogUseCase;

#region Blog state reducers

// ReSharper disable once UnusedMember.Global
public sealed class FetchPostReducer : Reducer<BlogState, FetchPostAction>
{
    public override BlogState Reduce(BlogState state, FetchPostAction action) =>
        new(StoreState.Loading, null);
}

// ReSharper disable once UnusedMember.Global
public sealed class FetchPostReadyReducer : Reducer<BlogState, FetchPostReadyAction>
{
    public override BlogState Reduce(BlogState state, FetchPostReadyAction action) =>
        new(StoreState.Success, action.Post);
}

// ReSharper disable once UnusedMember.Global
public sealed class FetchPostFailedReducer : Reducer<BlogState, FetchPostFailedAction>
{
    public override BlogState Reduce(BlogState state, FetchPostFailedAction action) => 
        new(StoreState.Failed, null);
}

#endregion


#region Comments state reducers

// ReSharper disable once UnusedMember.Global
public sealed class FetchRootCommentsReducer : Reducer<CommentsState, FetchRootCommentsAction>
{
    public override CommentsState Reduce(CommentsState state, FetchRootCommentsAction action) =>
        new(action.PostKey, CommentsCollection.Loading, action.PageNumber, action.PageSize);
}

// ReSharper disable once UnusedMember.Global
public sealed class FetchRootCommentsReadyReducer : Reducer<CommentsState, FetchRootCommentsReadyAction>
{
    public override CommentsState Reduce(CommentsState state, FetchRootCommentsReadyAction action)
    {
        var comments = new CommentModel[action.Comments.Count];

        for (var index = 0; index < action.Comments.Count; index++)
        {
            comments[index] = action.Comments[index].MapComment(CommentsCollection.Unknown);
        }

        var commentsCollection = new CommentsCollection(CommentsCollectionState.Success, comments);

        return new(action.PostKey, commentsCollection, action.PageNumber, action.PageSize);
    }
}

// ReSharper disable once UnusedMember.Global
public sealed class FetchChildCommentsReducer : Reducer<CommentsState, FetchChildCommentsAction>
{
    public override CommentsState Reduce(CommentsState state, FetchChildCommentsAction action)
    {
        var comments = MapReplace(state.Comments, action);
        return new(state.PostKey, comments, state.PageNumber, state.PageSize);
    }

    private static CommentsCollection MapReplace(CommentsCollection sourceComments, FetchChildCommentsAction action)
    {
        var result = new ICommentBase[sourceComments.Count];

        for (var sourceIndex = 0; sourceIndex < sourceComments.Count; sourceIndex++)
        {
            if (sourceComments[sourceIndex] is CommentModel source)
            {
                var comments = source.Comments;

                if (source.Key == action.ParentKey)
                {
                    comments = new CommentsCollection(CommentsCollectionState.Loading, CommentsCollection.Unknown);
                }
                else if (0 < source.Comments.Count)
                {
                    comments = MapReplace(source.Comments, action);
                }

                result[sourceIndex] = source.MapReplace(comments);

                continue;
            }

            result[sourceIndex] = sourceComments[sourceIndex];
        }

        return new CommentsCollection(sourceComments.State, result);
    }
}

// ReSharper disable once UnusedMember.Global
public sealed class FetchChildCommentsReadyReducer : Reducer<CommentsState, FetchChildCommentsReadyAction>
{
    public override CommentsState Reduce(CommentsState state, FetchChildCommentsReadyAction action)
    {
        var comments = MapReplace(state.Comments, action);
        return new(action.PostKey, comments, state.PageNumber, state.PageSize);
    }

    private static CommentsCollection MapReplace(CommentsCollection sourceComments, FetchChildCommentsReadyAction action)
    {
        var result = new ICommentBase[sourceComments.Count];

        for (var sourceIndex = 0; sourceIndex < sourceComments.Count; sourceIndex++)
        {
            if (sourceComments[sourceIndex] is CommentModel source)
            {
                var comments = source.Comments;

                if (source.Key == action.ParentKey)
                {
                    var childComments = new CommentModel[action.Comments.Count];

                    for (var index = 0; index < action.Comments.Count; index++)
                    {
                        childComments[index] = action.Comments[index].MapComment(CommentsCollection.Unknown);
                    }

                    comments = new CommentsCollection(CommentsCollectionState.Success, childComments);
                }
                else if (0 < source.Comments.Count)
                {
                    comments = MapReplace(source.Comments, action);
                }

                result[sourceIndex] = source.MapReplace(comments);

                continue;
            }

            result[sourceIndex] = sourceComments[sourceIndex];
        }

        return new CommentsCollection(sourceComments.State, result);
    }
}

// ReSharper disable once UnusedMember.Global
public sealed class FetchCommentsFailedReducer : Reducer<CommentsState, FetchCommentsFailedAction>
{
    public override CommentsState Reduce(CommentsState state, FetchCommentsFailedAction action)
    {
        var comments = MapReplace(state.Comments, action);
        return new(action.PostKey, comments, state.PageNumber, state.PageSize);
    }

    private static CommentsCollection MapReplace(CommentsCollection sourceComments, FetchCommentsFailedAction action)
    {
        var result = new ICommentBase[sourceComments.Count];

        for (var sourceIndex = 0; sourceIndex < sourceComments.Count; sourceIndex++)
        {
            if (sourceComments[sourceIndex] is CommentModel source)
            {
                var comments = source.Comments;

                if (source.Key == action.ParentKey)
                {
                    comments = new CommentsCollection(CommentsCollectionState.Failed, CommentsCollection.Unknown);
                }
                else if (0 < source.Comments.Count)
                {
                    comments = MapReplace(source.Comments, action);
                }

                result[sourceIndex] = source.MapReplace(comments);

                continue;
            }
            
            result[sourceIndex] = sourceComments[sourceIndex];
        }

        return new CommentsCollection(sourceComments.State, result);
    }
}

// ReSharper disable once UnusedMember.Global
public sealed class PublishReplyReducer : Reducer<CommentsState, PublishReplyAction>
{
    public override CommentsState Reduce(CommentsState state, PublishReplyAction action)
    {
        var comments = Reduce(state.Comments, action);
        return new(state.PostKey, comments, state.PageNumber, state.PageSize);
    }

    private static CommentsCollection MapReplace(CommentsCollection sourceComments, PublishReplyAction action)
    {
        var result = new ICommentBase[sourceComments.Count];

        for (var sourceIndex = 0; sourceIndex < sourceComments.Count; sourceIndex++)
        {
            if (sourceComments[sourceIndex] is CommentModel source)
            {
                var comments = source.Comments;

                if (source.Key == action.ParentKey)
                {
                    var commentReply = new CommentReplyModel(
                        action.ParentKey,
                        action.CorrelationId,
                        authorId: null,
                        CommentReplyResult.Publishing,
                        DateTimeOffset.MinValue
                    );
                    comments = source.Comments.Append(commentReply, source.Comments.State);
                }
                else if (0 < source.Comments.Count)
                {
                    comments = MapReplace(source.Comments, action);
                }

                result[sourceIndex] = source.MapReplace(comments);

                continue;
            }

            result[sourceIndex] = sourceComments[sourceIndex];
        }

        return new CommentsCollection(sourceComments.State, result);
    }

    private static CommentsCollection Reduce(CommentsCollection comments, PublishReplyAction action)
    {
        if (null == action.ParentKey)
        {
            var commentReply = new CommentReplyModel(
                action.ParentKey,
                action.CorrelationId,
                authorId: null,
                CommentReplyResult.Publishing,
                DateTimeOffset.MinValue
            );

            return comments.Append(commentReply, comments.State);
        }

        return MapReplace(comments, action);
    }
}

// ReSharper disable once UnusedMember.Global
public sealed class PublishReplySuccessReducer : Reducer<CommentsState, PublishReplySuccessAction>
{
    public override CommentsState Reduce(CommentsState state, PublishReplySuccessAction action)
    {
        var comments = MapReplace(state.Comments, action);
        return new(state.PostKey, comments, state.PageNumber, state.PageSize);
    }

    private static CommentsCollection MapReplace(CommentsCollection sourceComments, PublishReplySuccessAction action)
    {
        var result = new ICommentBase[sourceComments.Count];
        var state = sourceComments.State;

        for (var sourceIndex = 0; sourceIndex < sourceComments.Count; sourceIndex++)
        {
            if (sourceComments[sourceIndex] is CommentReplyModel reply)
            {
                if (reply.CorrelationKey == action.CorrelationId)
                {
                    state = CommentsCollectionState.Success;
                    result[sourceIndex] = new NewCommentModel(
                        action.Comment.Key,
                        action.Comment.PostKey,
                        action.Comment.ParentKey,
                        action.Comment.Text,
                        action.Comment.AuthorId,
                        CommentsCollection.Unknown,
                        action.Comment.CreatedAt,
                        action.CorrelationId
                    );
                }
                else
                {
                    result[sourceIndex] = reply;
                }
            }
            else if(sourceComments[sourceIndex] is CommentModel comment)
            {
                result[sourceIndex] = comment.MapReplace(MapReplace(comment.Comments, action));
            }
            else
            {
                result[sourceIndex] = sourceComments[sourceIndex];
            }
        }

        return new CommentsCollection(state, result);
    }
}

// ReSharper disable once UnusedMember.Global
public sealed class PublishReplyFailedReducer : Reducer<CommentsState, PublishReplyFailedAction>
{
    public override CommentsState Reduce(CommentsState state, PublishReplyFailedAction action)
    {
        var comments = MapReplace(state.Comments, action);
        return new(state.PostKey, comments, state.PageNumber, state.PageSize);
    }

    private static CommentsCollection MapReplace(CommentsCollection sourceComments, PublishReplyFailedAction action)
    {
        var result = new ICommentBase[sourceComments.Count];

        for (var sourceIndex = 0; sourceIndex < sourceComments.Count; sourceIndex++)
        {
            if (sourceComments[sourceIndex] is CommentReplyModel reply && reply.CorrelationKey == action.CorrelationId && CommentReplyResult.Publishing == reply.Result)
            {
                result[sourceIndex] = new CommentReplyModel(
                    reply.ParentKey,
                    reply.CorrelationKey,
                    reply.AuthorId,
                    CommentReplyResult.Failed,
                    reply.CreateAt
                );
            }

            if (sourceComments[sourceIndex] is CommentModel comment)
            {
                result[sourceIndex] = comment.MapReplace(MapReplace(comment.Comments, action));
            }
            else
            {
                result[sourceIndex] = sourceComments[sourceIndex];
            }
        }

        return new CommentsCollection(sourceComments.State, result.AsReadOnly());
    }
}

#endregion
