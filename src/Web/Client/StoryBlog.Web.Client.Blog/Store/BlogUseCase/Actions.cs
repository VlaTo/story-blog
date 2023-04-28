using StoryBlog.Web.Client.Blog.Core;
using StoryBlog.Web.Microservices.Comments.Shared.Models;
using StoryBlog.Web.Microservices.Posts.Shared.Models;

namespace StoryBlog.Web.Client.Blog.Store.BlogUseCase;

// Blog load actions graph
// FetchPostReferenceAction (slug => post key) -> FetchPostAction -> FetchPostReadyAction -> FetchCommentsAction
// -> FetchCommentsReadyAction

#region Blog actions

/// <summary>
/// 
/// </summary>
public sealed record FetchPostReferenceAction(string Slug);

/// <summary>
/// 
/// </summary>
public sealed record FetchPostAction(Guid Key);

/// <summary>
/// 
/// </summary>
public sealed record FetchPostReadyAction(Guid Key, PostModel Post);

/// <summary>
/// 
/// </summary>
public sealed record FetchPostFailedAction();

#endregion

#region Comments actions

/// <summary>
/// 
/// </summary>
public sealed record FetchRootCommentsAction(Guid PostKey, int PageNumber, int PageSize);

/// <summary>
/// 
/// </summary>
public sealed record FetchChildCommentsAction(Guid PostKey, Guid ParentKey);

/// <summary>
/// 
/// </summary>
/// <param name="PostKey"></param>
/// <param name="PageNumber"></param>
/// <param name="PageSize"></param>
/// <param name="Comments"></param>
public sealed record FetchRootCommentsReadyAction(Guid PostKey, int PageNumber, int PageSize, IReadOnlyList<CommentModel> Comments);

/// <summary>
/// 
/// </summary>
/// <param name="PostKey"></param>
/// <param name="ParentKey"></param>
/// <param name="Comments"></param>
public sealed record FetchChildCommentsReadyAction(Guid PostKey, Guid ParentKey, IReadOnlyList<CommentModel> Comments);

/// <summary>
/// 
/// </summary>
public sealed record FetchCommentsFailedAction(Guid PostKey, Guid? ParentKey);

#endregion