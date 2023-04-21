using Microsoft.AspNetCore.Components;
using MudBlazor.Utilities;
using StoryBlog.Web.Microservices.Comments.Shared.Models;

namespace StoryBlog.Web.Client.Blog.Components;

public partial class Comment
{
    protected string Classname => new CssBuilder("storyblog-blog-comment")
        .AddClass(Class)
        .Build();

    [Parameter]
    public string? Class
    {
        get;
        set;
    }

    [Parameter]
    public Guid Key
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
    public IReadOnlyList<CommentModel>? Comments
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
}