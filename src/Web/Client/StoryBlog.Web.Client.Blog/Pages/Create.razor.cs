using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using StoryBlog.Web.Client.Blog.Models;
using StoryBlog.Web.Client.Blog.Store;
using StoryBlog.Web.Client.Blog.Store.CreatePostUseCase;

namespace StoryBlog.Web.Client.Blog.Pages;

public partial class Create
{
    [Inject]
    public IState<CreatePostState> Post
    {
        get;
        set;
    }

    [Inject]
    public IState<SlugState> Slug
    {
        get;
        set;
    }

    [Inject]
    public IDispatcher Dispatcher
    {
        get;
        set;
    }

    private NewPostModel Model
    {
        get;
        set;
    }

    private string? Text
    {
        get;
        set;
    }

    private MudMarkdownStyling Styling
    {
        get;
        set;
    }

    private MudTextField<string> slugComponent;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        Text = "This component is defined in the <strong>StoryBlog.Web.Blazor.Markdown.Editor</strong> library.";
        Styling = new MudMarkdownStyling();
        Model = new NewPostModel
        {
            Title = String.Empty,
            Slug = null,
            Text = "Lorem Ipsum dolor sit amet"
        };

        Post.StateChanged += OnPostChanged;
        Slug.StateChanged += OnSlugChanged;
    }

    private void OnPostChanged(object? sender, EventArgs e)
    {
        Model.Title = Post.Value.Title;

        StateHasChanged();
    }

    private void OnSlugChanged(object? sender, EventArgs e)
    {
        switch (Slug.Value.StoreState)
        {
            case StoreState.Failed:
            {
                slugComponent.Disabled = false;

                break;
            }

            case StoreState.Empty:
            {
                slugComponent.Disabled = false;

                break;
            }

            case StoreState.Loading:
            {
                slugComponent.Disabled = true;

                break;
            }

            case StoreState.Success:
            {
                slugComponent.Disabled = false;

                break;
            }
        }

        Model.Slug = Slug.Value.Slug;

        StateHasChanged();
    }

    private void OnGenerateSlug(MouseEventArgs e)
    {
        Dispatcher.Dispatch(new GenerateSlugAction(Model.Title));
    }

    private void OnSubmitForm(EditContext context)
    {
        Dispatcher.Dispatch(new CreatePostAction(Model.Title, Model.Slug!, Text!));
    }

    private void OnInvalidForm(EditContext context)
    {
        ;
    }
}