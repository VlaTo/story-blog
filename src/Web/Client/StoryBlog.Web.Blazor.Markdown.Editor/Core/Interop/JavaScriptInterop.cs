using Microsoft.JSInterop;

namespace StoryBlog.Web.Blazor.Markdown.Editor.Core.Interop;

internal sealed class JavaScriptInterop : IJavaScriptInterop
{
    private readonly Lazy<Task<IJSObjectReference>> moduleTask;

    public JavaScriptInterop(IJSRuntime jsRuntime)
    {
        moduleTask = new(() => jsRuntime
            .InvokeAsync<IJSObjectReference>(
                "import",
                "./_content/StoryBlog.Web.Blazor.Markdown.Editor/StoryBlog.Web.Blazor.Markdown.Editor.js")
            .AsTask()
        );
    }

    public async ValueTask<IJavaScriptMarkdownEditor> CreateEditorAsync(string selector, string uniqueKey)
    {
        var module = await moduleTask.Value;
        var key = await module.InvokeAsync<string>("createEditor", selector, uniqueKey);
        return new JavaScriptMarkdownEditor(module, $"editors.{key}");
    }

    public async ValueTask DisposeAsync()
    {
        if (moduleTask.IsValueCreated)
        {
            var module = await moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}