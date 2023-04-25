namespace StoryBlog.Web.Microservices.Comments.Application.Models;

public class CreateCommentDetails
{
    public Guid PostKey
    {
        get;
        set;
    }

    public Guid? ParentKey
    {
        get;
        set;
    }

    public string Text
    {
        get;
        set;
    }
}