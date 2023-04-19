namespace StoryBlog.Web.Blazor.Markdown.Editor.Core.Interop;

public interface IJavaScriptInterop : IAsyncDisposable
{
    ValueTask<IJavaScriptMarkdownEditor> CreateEditorAsync(string selector);
}