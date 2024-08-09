namespace StoryBlog.Web.Blazor.Markdown.Editor.Core.Interop;

public interface IJavaScriptMarkdownEditor : IAsyncDisposable
{
    ValueTask SetTextAsync(string value);

    ValueTask<string> GetTextAsync();
}