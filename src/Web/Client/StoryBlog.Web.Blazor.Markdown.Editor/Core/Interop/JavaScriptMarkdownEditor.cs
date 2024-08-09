using Microsoft.JSInterop;

namespace StoryBlog.Web.Blazor.Markdown.Editor.Core.Interop;

internal sealed class JavaScriptMarkdownEditor : IJavaScriptMarkdownEditor
{
    private readonly IJSObjectReference jsRuntime;
    private readonly string identifier;

    public JavaScriptMarkdownEditor(IJSObjectReference jsRuntime, string identifier)
    {
        this.jsRuntime = jsRuntime;
        this.identifier = identifier;
    }

    public ValueTask SetTextAsync(string value) =>
        jsRuntime.InvokeVoidAsync(identifier + ".setText", value);

    public ValueTask<string> GetTextAsync() =>
        jsRuntime.InvokeAsync<string>(identifier + ".getText");

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}