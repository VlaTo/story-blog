using System.Windows.Input;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;

namespace StoryBlog.Web.Blazor.Markdown.Editor.Core.Components;

public interface IToolbarButton : IToolbarItem
{
    string Title
    {
        get; 
        set;
    }

    ICommand Command
    {
        get;
        set;
    }

    EventCallback<MouseEventArgs> OnClick
    {
        get; 
        set;
    }
}