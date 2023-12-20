namespace StoryBlog.Web.Client.Blog.Models;

public interface ICommentBase
{
    Guid? ParentKey
    {
        get;
    }
    
    DateTimeOffset CreateAt
    {
        get;
    }
}