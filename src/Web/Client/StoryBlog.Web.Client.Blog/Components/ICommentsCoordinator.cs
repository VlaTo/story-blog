namespace StoryBlog.Web.Client.Blog.Components;

public interface ICommentsCoordinator
{
    IDisposable Subscribe(ICommentsObserver observer);

    void NotifyReplyComposerOpened(ICommentsObserver observer);

    Task FetchCommentsAsync(Guid postKey, Guid parentKey);

    Task PublishReplyAsync(Guid postKey, Guid parentKey, string reply);
}