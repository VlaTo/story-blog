using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace StoryBlog.Web.Client.Blog.Pages;

[AllowAnonymous]
public partial class Authentication
{
    [Parameter]
    public string? Action
    {
        get;
        set;
    }
}