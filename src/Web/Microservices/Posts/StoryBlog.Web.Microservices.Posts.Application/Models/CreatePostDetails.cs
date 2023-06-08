namespace StoryBlog.Web.Microservices.Posts.Application.Models;

public sealed class CreatePostDetails : PostDetails
{
    public string Text
    {
        get;
        init;
    }

    public string Brief
    {
        get;
        init;
    }
}