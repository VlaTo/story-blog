using Microsoft.AspNetCore.Components;
using StoryBlog.Web.Blazor.Markdown.Editor.Core;
using StoryBlog.Web.Blazor.Markdown.Editor.Core.Components;
using StoryBlog.Web.Blazor.Markdown.Editor.Core.Extensions;

namespace StoryBlog.Web.Blazor.Markdown.Editor;

public partial class ToolbarGroup
{
    private static readonly ClassBuilder<ToolbarGroup> classBuilder;

    [Parameter]
    public RenderFragment ChildContent
    {
        get;
        set;
    }

    [CascadingParameter(Name = nameof(Editor))]
    public IMarkdownEditorContext Editor
    {
        get;
        set;
    }

    protected string Classname
    {
        get;
        private set;
    }

    static ToolbarGroup()
    {
        classBuilder = ClassBuilder
            .CreateFor<ToolbarGroup>()
            .DefineClass(builder => builder.Name("md-editor-toolbar-item").NoPrefix())
            .DefineClass(builder => builder.Name("md-editor-toolbar-group").NoPrefix())
            .DefineClass(builder => builder.Name("disabled").NoPrefix().Condition(group => group.Disabled));
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        UpdateState();
    }

    protected override void UpdateState()
    {
        Classname = classBuilder.Build(this);
    }
}