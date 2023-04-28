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
        new(StoreState.Loading, action.PostKey, Array.Empty<CommentModel>(), action.PageNumber, action.PageSize);
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

        return new(StoreState.Success, action.PostKey, comments, action.PageNumber, action.PageSize);
    }
}

// ReSharper disable once UnusedMember.Global
public sealed class FetchChildCommentsReducer : Reducer<CommentsState, FetchChildCommentsAction>
{
    public override CommentsState Reduce(CommentsState state, FetchChildCommentsAction action)
    {
        var comments = MapReplaceComments(state.Comments, action);
        return new(StoreState.Success, action.PostKey, action.ParentKey, comments);
    }

    private static IReadOnlyList<CommentModel> MapReplaceComments(
        IReadOnlyList<CommentModel> sourceComments,
        FetchChildCommentsAction action)
    {
        var result = new CommentModel[sourceComments.Count];

        for (var sourceIndex = 0; sourceIndex < sourceComments.Count; sourceIndex++)
        {
            var source = sourceComments[sourceIndex];
            var comments = source.Comments;

            if (source.PostKey == action.PostKey && source.Key == action.ParentKey)
            {
                comments = new CommentsCollection(CommentsCollectionState.Loading, null);
            }
            else if (0 < source.Comments.Count)
            {
                comments = MapReplaceChildComments(source.Comments, action);
            }

            result[sourceIndex] = source.ReplaceComments(comments);
        }

        return result;
    }

    private static CommentsCollection MapReplaceChildComments(CommentsCollection sourceComments, FetchChildCommentsAction action)
    {
        return new CommentsCollection(sourceComments.State, MapReplaceComments(sourceComments, action));
    }
}

// ReSharper disable once UnusedMember.Global
public sealed class FetchChildCommentsReadyReducer : Reducer<CommentsState, FetchChildCommentsReadyAction>
{
    public override CommentsState Reduce(CommentsState state, FetchChildCommentsReadyAction action)
    {
        var comments = MapReplaceComments(state.Comments, action);
        return new(StoreState.Success, action.PostKey, action.ParentKey, comments);
    }

    private static IReadOnlyList<CommentModel> MapReplaceComments(
        IReadOnlyList<CommentModel> sourceComments,
        FetchChildCommentsReadyAction action)
    {
        var result = new CommentModel[sourceComments.Count];

        for (var sourceIndex = 0; sourceIndex < sourceComments.Count; sourceIndex++)
        {
            var source = sourceComments[sourceIndex];
            var comments = source.Comments;

            if (source.PostKey == action.PostKey && source.Key == action.ParentKey)
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
                comments = MapReplaceChildComments(source.Comments, action);
            }

            result[sourceIndex] = source.ReplaceComments(comments);
        }

        return result;
    }

    private static CommentsCollection MapReplaceChildComments(CommentsCollection sourceComments, FetchChildCommentsReadyAction action)
    {
        return new CommentsCollection(sourceComments.State, MapReplaceComments(sourceComments, action));
    }
}

// ReSharper disable once UnusedMember.Global
public sealed class FetchCommentsFailedReducer : Reducer<CommentsState, FetchCommentsFailedAction>
{
    public override CommentsState Reduce(CommentsState state, FetchCommentsFailedAction action) =>
        new(StoreState.Success, action.PostKey, action.ParentKey, Array.Empty<CommentModel>());
}

#endregion
