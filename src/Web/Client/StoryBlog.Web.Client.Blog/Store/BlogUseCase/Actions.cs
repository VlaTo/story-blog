using StoryBlog.Web.Microservices.Comments.Shared.Models;
using StoryBlog.Web.Microservices.Posts.Shared.Models;

namespace StoryBlog.Web.Client.Blog.Store.BlogUseCase;

// Blog load actions graph
// FetchPostAction -> FetchPostReadyAction -> FetchCommentsAction -> FetchCommentsReadyAction

#region Blog actions

/// <summary>
/// 
/// </summary>
public sealed record FetchPostAction(string SlugOrKey);

/// <summary>
/// 
/// </summary>
public sealed record FetchPostReadyAction(string SlugOrKey, PostModel Post);

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
public sealed record FetchRootCommentsReadyAction(Guid PostKey, int PageNumber, int PageSize, IReadOnlyList<CommentModel> Comments);

/// <summary>
/// 
/// </summary>
public sealed record FetchChildCommentsReadyAction(Guid PostKey, Guid ParentKey, IReadOnlyList<CommentModel> Comments);

/// <summary>
/// 
/// </summary>
public sealed record FetchCommentsFailedAction(Guid PostKey, Guid? ParentKey);

/// <summary>
/// 
/// </summary>
public sealed record PublishReplyAction(Guid PostKey, Guid? ParentKey, string CorrelationId, string Reply);

/// <summary>
/// 
/// </summary>
public sealed record PublishReplySuccessAction(CreatedCommentModel Comment, string CorrelationId);

/// <summary>
/// 
/// </summary>
public sealed record PublishReplyFailedAction(Guid PostKey, Guid? ParentKey, string CorrelationId);

#endregion