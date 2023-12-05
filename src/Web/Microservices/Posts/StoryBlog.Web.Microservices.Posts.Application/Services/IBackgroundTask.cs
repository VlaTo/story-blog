namespace StoryBlog.Web.Microservices.Posts.Application.Services;

public interface IBackgroundTask
{
    Guid TaskKey
    {
        get;
    }

    Guid PostKey
    {
        get;
    }
}