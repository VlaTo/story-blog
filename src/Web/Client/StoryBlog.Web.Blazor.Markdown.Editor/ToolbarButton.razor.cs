using System.Windows.Input;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using StoryBlog.Web.Blazor.Markdown.Editor.Core;
using StoryBlog.Web.Blazor.Markdown.Editor.Core.Components;
using StoryBlog.Web.Blazor.Markdown.Editor.Core.Extensions;

namespace StoryBlog.Web.Blazor.Markdown.Editor;

public partial class ToolbarButton : IMarkdownEditorObserver, IToolbarButton
{
    private static readonly ClassBuilder<ToolbarButton> classBuilder;
    private IDisposable? editorSubscription;

    [Parameter]
    public string Title
    {
        get; 
        set;
    }

    [Parameter]
    public ICommand? Command
    {
        get; 
        set;
    }

    [Parameter]
    public object? CommandParameter
    {
        get;
        set;
    }

    [Parameter]
    public EventCallback<MouseEventArgs> OnClick
    {
        get;
        set;
    }

    [CascadingParameter(Name = nameof(Editor))]
    public IMarkdownEditorContext? Editor
    {
        get;
        set;
    }

    protected string Classname
    {
        get;
        private set;
    }

    public ToolbarButton()
    {
        OnClick = EventCallback<MouseEventArgs>.Empty;
    }

    static ToolbarButton()
    {
        classBuilder = ClassBuilder
            .CreateFor<ToolbarButton>()
            .DefineClass(builder => builder.Name("md-editor-toolbar-item").NoPrefix())
            .DefineClass(builder => builder.Name("md-editor-toolbar-button").NoPrefix())
            .DefineClass(builder => builder.Name("disabled").NoPrefix().Condition(button => button.Disabled));
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (null != Editor)
        {
            editorSubscription = Editor.Subscribe(this);
        }

        UpdateState();
    }

    protected async Task OnClickCallback(MouseEventArgs e)
    {
        if (null != Command)
        {
            if (false == Disabled && Command.CanExecute(CommandParameter))
            {
                Command.Execute(CommandParameter);
            }
        }

        await OnClick.InvokeAsync(e);
    }

    protected virtual void OnEditorInitialized()
    {
    }

    protected override void UpdateState()
    {
        Classname = classBuilder.Build(this);
    }

    #region IMarkdownEditorObserver

    void IMarkdownEditorObserver.OnInitialized()
    {
        OnEditorInitialized();
    }

    #endregion
}