using System.Windows.Input;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using MudBlazor.Utilities;

namespace StoryBlog.Web.Client.Blog.Components;

public partial class StoryBlogView
{
    protected string Classname => new CssBuilder("storyblog-blog-view")
        .AddClass("px-6")
        .AddClass("py-4")
        .AddClass("my-5")
        .AddClass(Class)
        .Build();

    [Parameter]
    public string? Class
    {
        get;
        set;
    }

    [Parameter]
    public string Title
    {
        get;
        set;
    }

    [Parameter]
    public string Slug
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

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object?> UserAttributes
    {
        get; 
        set;
    }

    [Inject]
    public NavigationManager NavigationManager
    {
        get;
        set;
    }

    public void OnTitleClick()
    {
        NavigationManager.NavigateTo($"/blog/{Slug}");
    }
}