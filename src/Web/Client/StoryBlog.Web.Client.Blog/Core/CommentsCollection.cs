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

public sealed class CommentsCollection : IReadOnlyList<ICommentBase>
{
    public static readonly CommentsCollection Unknown;
    
    public static readonly CommentsCollection Empty;

    private readonly IReadOnlyList<ICommentBase>? comments;
    
    public int Count => comments?.Count ?? 0;

    public CommentsCollectionState State
    {
        get;
    }

    public ICommentBase this[int index]
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

    public CommentsCollection(CommentsCollectionState state, IReadOnlyList<ICommentBase>? comments)
    {
        State = state;
        this.comments = comments;
    }

    static CommentsCollection()
    {
        Unknown = new CommentsCollection(CommentsCollectionState.Unknown, null);
        Empty = new CommentsCollection(CommentsCollectionState.Success, Array.Empty<ICommentBase>());
    }

    public CommentsCollection Append(ICommentBase comment, CommentsCollectionState state)
    {
        var source = comments?.ToArray() ?? Array.Empty<CommentModel>();
        var destination = new ICommentBase[source.Length + 1];

        Array.Copy(source, destination, Count);
        destination[Count] = comment;

        return new CommentsCollection(state, destination);
    }

    public IEnumerator<ICommentBase> GetEnumerator() => comments?.GetEnumerator() ?? EmptyEnumerator.Instance;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #region Empty enumerator

    /// <summary>
    /// 
    /// </summary>
    private sealed class EmptyEnumerator : IEnumerator<ICommentBase>
    {
        public static readonly IEnumerator<ICommentBase> Instance;

        public ICommentBase Current => throw new InvalidOperationException();

        object IEnumerator.Current => Current;

        public bool MoveNext() => false;

        static EmptyEnumerator()
        {
            Instance = new EmptyEnumerator();
        }

        public void Reset()
        {
        }

        public void Dispose()
        {
        }
    }

    #endregion
}