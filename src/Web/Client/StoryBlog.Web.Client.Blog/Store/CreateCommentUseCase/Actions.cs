namespace StoryBlog.Web.Client.Blog.Store.CreateCommentUseCase;

/// <summary>
/// 
/// </summary>
/// <param name="Text"></param>
/// <param name="DateTime"></param>
public sealed record CreateCommentAction(Guid PostKey, Guid? ParentKey, string Text, DateTime DateTime);