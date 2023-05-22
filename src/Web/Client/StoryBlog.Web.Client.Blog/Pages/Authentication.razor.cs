using Microsoft.AspNetCore.Components;

namespace StoryBlog.Web.Client.Blog.Pages;

public partial class Authentication
{
    [Parameter]
    public string? Action
    {
        get;
        set;
    }
}