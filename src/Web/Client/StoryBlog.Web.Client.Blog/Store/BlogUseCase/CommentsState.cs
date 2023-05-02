using Fluxor;
using StoryBlog.Web.Client.Blog.Core;

namespace StoryBlog.Web.Client.Blog.Store.BlogUseCase;

/// <summary>
/// 
/// </summary>
[FeatureState]
public sealed class CommentsState
{
    public Guid PostKey
    {
        get;
    }

    public CommentsCollection Comments
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

    public CommentsState()
        : this(Guid.Empty, CommentsCollection.Unknown, -1, 0)
    {
    }

    public CommentsState(Guid postKey, CommentsCollection comments, int pageNumber, int pageSize)
    {
        PostKey = postKey;
        Comments = comments;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}