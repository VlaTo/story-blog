using StoryBlog.Web.Microservices.Posts.Application.Services;

namespace StoryBlog.Web.Microservices.Posts.Application.Models;

public sealed class BackgroundTask : IBackgroundTask
{
    public Guid TaskKey
    {
        get;
    }

    public Guid PostKey
    {
        get;
    }

    public BackgroundTask(Guid taskKey, Guid postKey)
    {
        TaskKey = taskKey;
        PostKey = postKey;
    }
}