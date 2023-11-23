using StoryBlog.Web.Microservices.Posts.Application.Services;

namespace StoryBlog.Web.Microservices.Posts.Application.Models;

public sealed class BackgroundTask : IBackgroundTask
{
    public Guid Id
    {
        get;
    }

    public TimeSpan Timeout
    {
        get;
    }

    public BackgroundTask(Guid id, TimeSpan timeout)
    {
        Id = id;
        Timeout = timeout;
    }
}