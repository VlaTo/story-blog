namespace StoryBlog.Web.Client.Blog.Components;

public interface ICommentsCoordinator
{
    IDisposable Subscribe(ICommentsObserver observer);

    void NotifyReplyComposerOpened(ICommentsObserver observer);

    Task FetchCommentsAsync(Guid parentKey);

    Task PublishReplyAsync(Guid parentKey, string reply, string correlationKey);
}