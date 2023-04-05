using Microsoft.AspNetCore.Components;

namespace StoryBlog.Web.Blazor.Markdown.Editor;

public partial class MarkdownEditor
{
    public string? Value
    {
        get;
        set;
    }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object?> UserAttributes
    {
        get;
        set;
    }

    public EventHandler<string?> ValueChanged
    {
        get;
        set;
    }
}