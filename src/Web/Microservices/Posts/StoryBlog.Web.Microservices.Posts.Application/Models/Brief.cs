using StoryBlog.Web.Microservices.Posts.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Application.Models;

/// <summary>
/// 
/// </summary>
public sealed class Brief : PostDetails
{
    /// <summary>
    /// 
    /// </summary>
    public Guid Key
    {
        get;
        set;
    }

    /// <summary>
    /// 
    /// </summary>
    public string Text
    {
        get;
        set;
    }

    /// <summary>
    /// 
    /// </summary>
    public PostStatus Status
    {
        get;
        set;
    }

    /// <summary>
    /// 
    /// </summary>
    public string Author
    {
        get; 
        set;
    }

    /// <summary>
    /// 
    /// </summary>
    public AllowedActions AllowedActions
    {
        get;
        init;
    }

    /// <summary>
    /// 
    /// </summary>
    public long CommentsCount
    {
        get;
        set;
    }

    /// <summary>
    /// 
    /// </summary>
    public DateTimeOffset CreatedAt
    {
        get;
        set;
    }
}