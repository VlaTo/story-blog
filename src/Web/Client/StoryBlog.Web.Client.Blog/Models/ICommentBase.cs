namespace StoryBlog.Web.Client.Blog.Models;

public interface ICommentBase
{
    Guid? ParentKey
    {
        get;
    }
    
    DateTime CreateAt
    {
        get;
    }
}