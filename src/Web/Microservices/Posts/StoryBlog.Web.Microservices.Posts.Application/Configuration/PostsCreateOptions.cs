namespace StoryBlog.Web.Microservices.Posts.Application.Configuration;

public sealed class PostsCreateOptions
{
    public bool ApprovePostWhenCreated
    {
        get; 
        set;
    }
}