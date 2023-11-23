namespace StoryBlog.Web.Microservices.Posts.Application.Services;

public interface IBackgroundWorkManager
{
    ValueTask<IBackgroundTask> QueueNewTaskAsync(double value, CancellationToken cancellationToken);
}