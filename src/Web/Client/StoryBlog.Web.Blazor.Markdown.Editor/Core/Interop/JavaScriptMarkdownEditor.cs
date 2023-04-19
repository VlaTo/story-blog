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

    public async ValueTask SetTextAsync(string value)
    {
        await jsRuntime.InvokeVoidAsync(identifier + ".setText", value);
    }
}