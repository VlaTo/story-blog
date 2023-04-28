using System.Collections;
using StoryBlog.Web.Client.Blog.Models;

namespace StoryBlog.Web.Client.Blog.Core;

public enum CommentsCollectionState
{
    Failed = -1,
    Unknown,
    Loading,
    Success
}

public sealed class CommentsCollection : IReadOnlyList<CommentModel>
{
    public static readonly CommentsCollection Unknown;

    private readonly IReadOnlyList<CommentModel>? comments;
    
    public int Count => comments?.Count ?? 0;

    public CommentsCollectionState State
    {
        get;
    }

    public CommentModel this[int index]
    {
        get
        {
            if (null == comments)
            {
                throw new Exception();
            }

            return comments[index];
        }
    }

    public CommentsCollection(CommentsCollectionState state, IReadOnlyList<CommentModel>? comments)
    {
        State = state;
        this.comments = comments;
    }

    static CommentsCollection()
    {
        Unknown = new CommentsCollection(CommentsCollectionState.Unknown, null);
    }

    public IEnumerator<CommentModel> GetEnumerator() => comments.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}