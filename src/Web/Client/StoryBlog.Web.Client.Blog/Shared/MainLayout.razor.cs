using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using MudBlazor.Utilities;
using StoryBlog.Web.Client.Blog.Configuration;
using StoryBlog.Web.MessageHub.Client;
using StoryBlog.Web.Microservices.Communication.MessageHub.Messages;

namespace StoryBlog.Web.Client.Blog.Shared;

[AllowAnonymous]
public partial class MainLayout
{
    private MessageHubConnection? hub;

    [Inject]
    private ISnackbar Snackbar
    {
        get; 
        set;
    }

    [Inject]
    private IOptions<MessageHubOptions> Options
    {
        get;
        set;
    }

    private readonly MudTheme theme = new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = Colors.Gray.Default
        },
        Typography = new Typography
        {
            Default = new Default
            {
                FontFamily = new[] { "Montserrat", "Roboto", "sans-serif" }
            }
        }
    };

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        hub = new MessageHubConnectionBuilder()
            .WithUrl(Options.Value.ConnectionUri)
            .Build();

        hub.On<NewPostPublishedHubMessage>("post.created", message =>
        {
            Snackbar.Add("New Post created");
            return Task.CompletedTask;
        });

        /*hub.On<PostRemovedMessage>("post.removed", message =>
        {
            Snackbar.Add("Post deleted");
            return Task.CompletedTask;
        });*/

        await hub.ConnectAsync();
    }
}