namespace StoryBlog.Web.Microservices.Posts.Application.Services;

public interface IBackgroundTaskToken : IAsyncDisposable
{
    Guid TaskKey
    {
        get;
    }

    Guid PostKey
    {
        get;
    }

    ValueTask SuccessAsync(CancellationToken cancellationToken);
    
    ValueTask SkipAsync(CancellationToken cancellationToken);
}