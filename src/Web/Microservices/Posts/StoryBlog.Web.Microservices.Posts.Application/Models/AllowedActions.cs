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
    CanEdit = 0x01,

    /// <summary>
    /// 
    /// </summary>
    CanDelete = 0x02,

    /// <summary>
    /// 
    /// </summary>
    CanTogglePublic = 0x04
}