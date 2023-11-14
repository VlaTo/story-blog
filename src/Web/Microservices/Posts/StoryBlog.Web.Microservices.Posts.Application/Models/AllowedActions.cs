namespace StoryBlog.Web.Microservices.Posts.Application.Models;

/// <summary>
/// 
/// </summary>
[Flags]
public enum AllowedActions : byte
{
    /// <summary>
    /// 
    /// </summary>
    CanEdit,

    /// <summary>
    /// 
    /// </summary>
    CanDelete
}