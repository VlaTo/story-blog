namespace StoryBlog.Web.Microservices.Posts.Shared.Models;

public sealed class EditPostRequest
{
    public string Title
    {
        get;
        set;
    }

    public string Slug
    {
        get;
        set;
    }
}