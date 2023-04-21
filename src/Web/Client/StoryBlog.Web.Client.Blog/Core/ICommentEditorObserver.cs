namespace StoryBlog.Web.Client.Blog.Core;

public interface ICommentEditorObserver
{
    void OnEditorEventRaised(ICommentEditor editor);
}