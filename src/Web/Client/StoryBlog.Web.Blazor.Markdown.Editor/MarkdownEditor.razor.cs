using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using StoryBlog.Web.Blazor.Markdown.Editor.Core;
using StoryBlog.Web.Blazor.Markdown.Editor.Core.Extensions;
using StoryBlog.Web.Blazor.Markdown.Editor.Core.Interop;

namespace StoryBlog.Web.Blazor.Markdown.Editor;

public partial class MarkdownEditor
{
    private static readonly ClassBuilder<MarkdownEditor> classBuilder;
    private static readonly StyleBuilder<MarkdownEditor> styleBuilder;

    private ElementReference editorElement;
    private IJavaScriptMarkdownEditor? editor; 

    [Parameter]
    public string? Class
    {
        get;
        set;
    }

    [Parameter]
    public RenderFragment Toolbar
    {
        get;
        set;
    }

    [Parameter]
    public string? Text
    {
        get;
        set;
    }

    [Parameter]
    public string? Height
    {
        get;
        set;
    }

    [Parameter]
    public string? MinHeight
    {
        get;
        set;
    }

    [Parameter]
    public string? MaxHeight
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

    [Parameter]
    public EventCallback<string?> TextChanged
    {
        get;
        set;
    }

    [Inject]
    private IJavaScriptInterop EditorJavaScriptInterop
    {
        get;
        set;
    }

    protected string Classname
    {
        get;
        private set;
    }

    protected string EditorStyle
    {
        get;
        private set;
    }

    public MarkdownEditor()
    {
        Toolbar = DefaultToolbar;
        TextChanged = EventCallback<string?>.Empty;
    }

    static MarkdownEditor()
    {
        classBuilder = ClassBuilder
            .CreateFor<MarkdownEditor>()
            .DefineClass(builder => builder.Name("md-editor").NoPrefix());
        styleBuilder = StyleBuilder
            .CreateFor<MarkdownEditor>()
            .DefineProperty(property => property
                .Name("overflow-y")
                .Value("scroll")
            )
            .DefineProperty(property => property
                .Name("height")
                .ValueIfExists(x => x.Height)
            )
            .DefineProperty(property => property
                .Name("min-height")
                .ValueIfExists(x => x.MinHeight)
            )
            .DefineProperty(property => property
                .Name("max-height")
                .ValueIfExists(x => x.MaxHeight)
            );
    }

    public async ValueTask SetTextAsync(string value)
    {
        if (String.Equals(Text, value))
        {
            return;
        }

        Text = value;

        await TextChanged.InvokeAsync(value);
    }

    public override ValueTask DisposeAsync()
    {
        return editor?.DisposeAsync() ?? ValueTask.CompletedTask;
        //return EditorJavaScriptInterop.DisposeAsync();
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        UpdateState();
    }

    protected override Task OnParametersSetAsync()
    {
        return base.OnParametersSetAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (FirstRender)
        {
            var selector = $".md-editor div[_bl_{editorElement.Id}]";
            var uniqueEditorKey = Guid.NewGuid().ToString("N");

            editor = await EditorJavaScriptInterop.CreateEditorAsync(selector, uniqueEditorKey);

            if (String.IsNullOrEmpty(Text))
            {
                return;
            }

            await editor.SetTextAsync(Text);

            if (AutoFocus)
            {
                await editorElement.FocusAsync();
            }
        }
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        ;
    }

    protected override async ValueTask OnBlurAsync(FocusEventArgs e)
    {
        Text = await editor!.GetTextAsync();
        
        //await TextChanged.InvokeAsync(Text);

        if (IsDirty)
        {
            IsDirty = false;
            await TextChanged.InvokeAsync(Text);
        }
    }

    protected ValueTask OnInputAsync(ChangeEventArgs e)
    {
        IsDirty = true;

        return ValueTask.CompletedTask;
    }

    protected async ValueTask OnBeforeInputAsync(EventArgs e)
    {
        ;
    }

    protected virtual void UpdateState()
    {
        Classname = classBuilder.Build(this);
        EditorStyle = styleBuilder.Build(this);
    }

    private RenderFragment DefaultToolbar => (builder =>
    {
        builder.OpenElement(1, "div");

        builder.CloseElement();
    });
}