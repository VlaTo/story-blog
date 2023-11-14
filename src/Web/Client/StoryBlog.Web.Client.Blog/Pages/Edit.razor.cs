using Fluxor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using StoryBlog.Web.Client.Blog.Models;
using StoryBlog.Web.Client.Blog.Store.BlogUseCase;
using StoryBlog.Web.Client.Blog.Store.CreatePostUseCase;

namespace StoryBlog.Web.Client.Blog.Pages;

[Authorize(Policy = "Editor")]
public partial class Edit
{
    [Inject]
    public IDispatcher Dispatcher
    {
        get;
        set;
    }

    [Parameter]
    [SupplyParameterFromQuery]
    public string SlugOrKey
    {
        get;
        set;
    }

    private EditPostModel Model
    {
        get;
        set;
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        //Text = "This component is defined in the <strong>StoryBlog.Web.Blazor.Markdown.Editor</strong> library.";
        //Styling = new MudMarkdownStyling();
        Model = new EditPostModel
        {
            Title = String.Empty,
            Slug = null,
            //Text = "Lorem Ipsum dolor sit amet"
        };

        //Post.StateChanged += OnPostChanged;
        //Slug.StateChanged += OnSlugChanged;

        Dispatcher.Dispatch(new FetchPostAction(SlugOrKey));
    }

    private void OnSubmitForm(EditContext context)
    {
        Dispatcher.Dispatch(new CreatePostAction(Model.Title, Model.Slug!));
    }

    private void OnInvalidForm(EditContext context)
    {
        ;
    }

    private void OnGenerateSlug(MouseEventArgs e)
    {
        Dispatcher.Dispatch(new GenerateSlugAction(Model.Title));
    }
}