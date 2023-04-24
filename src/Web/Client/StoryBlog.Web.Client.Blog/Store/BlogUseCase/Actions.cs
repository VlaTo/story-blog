using StoryBlog.Web.Microservices.Comments.Shared.Models;
using StoryBlog.Web.Microservices.Posts.Shared.Models;

namespace StoryBlog.Web.Client.Blog.Store.BlogUseCase;

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

/// <summary>
/// 
/// </summary>
public sealed record FetchCommentsReadyAction(Guid PostKey, IReadOnlyCollection<CommentModel> Comments);

/// <summary>
/// 
/// </summary>
public sealed record FetchCommentsFailedAction();