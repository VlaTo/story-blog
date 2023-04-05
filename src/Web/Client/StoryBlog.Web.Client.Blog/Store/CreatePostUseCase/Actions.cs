using StoryBlog.Web.Microservices.Posts.Shared.Models;

namespace StoryBlog.Web.Client.Blog.Store.CreatePostUseCase;

/// <summary>
/// 
/// </summary>
public sealed class GenerateSlugAction
{
    public string Title
    {
        get;
    }

    public GenerateSlugAction(string title)
    {
        Title = title;
    }
}

/// <summary>
/// 
/// </summary>
public sealed class GeneratedSlugReadyAction
{
    public string Title
    {
        get;
    }

    public string Slug
    {
        get;
    }

    public GeneratedSlugReadyAction(string title, string slug)
    {
        Title = title;
        Slug = slug;
    }
}

/// <summary>
/// 
/// </summary>
public sealed class GenerateSlugFailedAction
{
    public string Title
    {
        get;
    }

    public GenerateSlugFailedAction(string title)
    {
        Title = title;
    }
}

/// <summary>
/// 
/// </summary>
public sealed class CreatePostAction
{
    public string Title
    {
        get;
    }

    public string Slug
    {
        get;
    }

    public CreatePostAction(string title, string slug)
    {
        Title = title;
        Slug = slug;
    }
}

/// <summary>
/// 
/// </summary>
public sealed class CreatedPostReadyAction
{
    public string Title
    {
        get;
    }

    public string Slug
    {
        get;
    }

    public CreatedPostReadyAction(
        string title,
        string slug,
        PostModelStatus status,
        DateTime createdAt)
    {
        Title = title;
        Slug = slug;
    }
}

/// <summary>
/// 
/// </summary>
public sealed class CreatePostFailedAction
{
    public CreatePostFailedAction()
    {
    }
}