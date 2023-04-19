namespace StoryBlog.Web.Blazor.Markdown.Editor.Core.Components;

public interface IMarkdownEditorContext
{
    IDisposable Subscribe(IMarkdownEditorObserver observer);

    bool IsActionEnabled(EditorAction action);
}