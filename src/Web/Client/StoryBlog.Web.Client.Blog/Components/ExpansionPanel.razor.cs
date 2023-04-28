using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor.Utilities;
using StoryBlog.Web.Client.Blog.Core;

namespace StoryBlog.Web.Client.Blog.Components;

public partial class ExpansionPanel
{
    private bool isExpanded;

    protected string Classname => new CssBuilder("storyblog-expansion-panel")
        .AddClass("expanded", () => IsExpanded)
        .AddClass(Class)
        .Build();

    protected string ContentPanelClassname => new CssBuilder("storyblog-expansion-panel-content")
        .AddClass("expanded", () => IsExpanded)
        .AddClass(Class)
        .Build();

    protected string Stylename => new StyleBuilder()
        .AddStyle("height", "auto", IsExpanded)
        .Build();

    [Parameter]
    public string Title
    {
        get;
        set;
    }

    [Parameter]
    public string? Class
    {
        get;
        set;
    }

    [Parameter]
    public RenderFragment? TitleContent
    {
        get;
        set;
    }

    [Parameter]
    public RenderFragment ChildContent
    {
        get;
        set;
    }

    [Parameter]
    public bool NoTitle
    {
        get;
        set;
    }

    [Parameter]
    public bool IsExpanded
    {
        get => isExpanded;
        set
        {
            if (isExpanded == value)
            {
                return ;
            }
            
            isExpanded = value;

            TaskRunner.Instance.QueueTask(IsExpandedChanged.InvokeAsync(value));
        }
    }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object?> UserAttributes
    {
        get;
        set;
    }

    [Parameter]
    public EventCallback<bool> IsExpandedChanged
    {
        get;
        set;
    }

    public void ToggleExpansion()
    {
        IsExpanded = false == IsExpanded;
    }

    private void DoHeaderClick(MouseEventArgs e)
    {
        ToggleExpansion();
    }
}