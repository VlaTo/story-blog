using Microsoft.AspNetCore.Components;

namespace StoryBlog.Web.Blazor.Markdown.Editor.Core.Components;

public abstract class ToolbarItemComponent : ComponentBase, IToolbarItem
{
    private bool disabled;

    [Parameter]
    public virtual bool Disabled
    {
        get => disabled;
        set
        {
            if (disabled == value)
            {
                return;
            }

            disabled = value;

            UpdateState();

            if (HasRendered)
            {
                StateHasChanged();
            }
        }
    }

    protected bool HasRendered
    {
        get;
        private set;
    }

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        if (firstRender)
        {
            HasRendered = true;
        }
    }

    protected abstract void UpdateState();
}