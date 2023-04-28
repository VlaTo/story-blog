using Fluxor;
using StoryBlog.Web.Client.Blog.Models;

namespace StoryBlog.Web.Client.Blog.Store.BlogUseCase;

/// <summary>
/// 
/// </summary>
public enum CommentsKind
{
    Root,
    Children
}

/// <summary>
/// 
/// </summary>
[FeatureState]
public sealed class CommentsState : AbstractStore
{
    public Guid PostKey
    {
        get;
    }

    public Guid? ParentKey
    {
        get;
    }

    public IReadOnlyList<CommentModel> Comments
    {
        get;
    }

    public int PageNumber
    {
        get;
    }

    public int PageSize
    {
        get;
    }

    public CommentsKind Kind
    {
        get;
    }

    public CommentsState()
        : this(StoreState.Empty, CommentsKind.Root, Guid.Empty, null, Array.Empty<CommentModel>(), -1, 0)
    {
    }

    public CommentsState(StoreState storeState, Guid postKey, IReadOnlyList<CommentModel> comments, int pageNumber, int pageSize)
        : this(storeState, CommentsKind.Root, postKey, null, comments, pageNumber, pageSize)
    {
    }

    public CommentsState(StoreState storeState, Guid postKey, Guid? parentKey, IReadOnlyList<CommentModel> comments)
        : this(storeState, CommentsKind.Children, postKey, parentKey, comments, -1, 0)
    {
    }

    private CommentsState(
        StoreState storeState,
        CommentsKind kind,
        Guid postKey,
        Guid? parentKey,
        IReadOnlyList<CommentModel> comments,
        int pageNumber,
        int pageSize)
        : base(storeState)
    {
        Kind = kind;
        PostKey = postKey;
        ParentKey = parentKey;
        Comments = comments;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}