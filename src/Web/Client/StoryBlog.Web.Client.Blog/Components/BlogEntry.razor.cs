using Microsoft.AspNetCore.Components;

namespace StoryBlog.Web.Client.Blog.Components;

public partial class BlogEntry
{
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
    public RenderFragment HeaderContent
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