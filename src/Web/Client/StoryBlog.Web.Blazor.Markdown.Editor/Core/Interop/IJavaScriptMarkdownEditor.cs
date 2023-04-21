namespace StoryBlog.Web.Blazor.Markdown.Editor.Core.Interop;

public interface IJavaScriptMarkdownEditor
{
    ValueTask SetTextAsync(string value);
    
    ValueTask<string> GetTextAsync();
}