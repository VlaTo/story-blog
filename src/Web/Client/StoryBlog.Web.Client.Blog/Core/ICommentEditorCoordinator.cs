namespace StoryBlog.Web.Client.Blog.Core;

public interface ICommentEditorCoordinator
{
    void RaiseEditorEvent(ICommentEditor editor);

    IDisposable Subscribe(ICommentEditorObserver observer);
}