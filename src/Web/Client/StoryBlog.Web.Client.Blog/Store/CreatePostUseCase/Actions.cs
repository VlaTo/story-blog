using StoryBlog.Web.Microservices.Posts.Shared.Models;

namespace StoryBlog.Web.Client.Blog.Store.CreatePostUseCase;

/// <summary>
/// 
/// </summary>
public sealed record GenerateSlugAction(string Title);

/// <summary>
/// 
/// </summary>
public sealed record GeneratedSlugReadyAction(string Title, string Slug);

/// <summary>
/// 
/// </summary>
public sealed record GenerateSlugFailedAction(string Title);

/// <summary>
/// 
/// </summary>
public sealed record CreatePostAction(string Title, string Slug);

/// <summary>
/// 
/// </summary>
public sealed record CreatedPostReadyAction(string Title, string Slug, PostModelStatus Status, DateTime CreatedAt);

/// <summary>
/// 
/// </summary>
public sealed record CreatePostFailedAction();