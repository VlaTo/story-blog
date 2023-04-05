using StoryBlog.Web.Microservices.Comments.Shared.Models;
using StoryBlog.Web.Microservices.Posts.Shared.Models;

namespace StoryBlog.Web.Client.Blog.Store.BlogUseCase;

/// <summary>
/// 
/// </summary>
public sealed class FetchPostReferenceAction    
{
    public string Slug
    {
        get;
    }

    public FetchPostReferenceAction(string slug)
    {
        Slug = slug;
    }
}

/// <summary>
/// 
/// </summary>
public sealed class FetchPostAction    
{
    public Guid Key
    {
        get;
    }

    public FetchPostAction(Guid key)
    {
        Key = key;
    }
}

/// <summary>
/// 
/// </summary>
public sealed class FetchPostReadyAction
{
    public Guid Key
    {
        get;
    }

    public PostModel Post
    {
        get;
    }

    public FetchPostReadyAction(Guid key, PostModel post)
    {
        Key = key;
        Post = post;
    }
}

/// <summary>
/// 
/// </summary>
public sealed class FetchPostFailedAction
{
    public FetchPostFailedAction()
    {
    }
}

/// <summary>
/// 
/// </summary>
public sealed class FetchCommentsReadyAction
{
    public Guid PostKey
    {
        get;
    }

    public IReadOnlyCollection<CommentModel> Comments
    {
        get;
    }

    public FetchCommentsReadyAction(Guid postKey, IReadOnlyCollection<CommentModel> comments)
    {
        PostKey = postKey;
        Comments = comments;
    }
}

/// <summary>
/// 
/// </summary>
public sealed class FetchCommentsFailedAction
{
    public FetchCommentsFailedAction()
    {
    }
}