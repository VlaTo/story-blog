using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using StoryBlog.Web.MessageHub.Client;
using StoryBlog.Web.Microservices.Posts.Shared.Messages;

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

    private readonly MudTheme theme = new()
    {
        Palette = new Palette
        {
            Primary = Colors.Grey.Default
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
            .WithUrl("ws://localhost:5033/notification")
            .Build();

        hub.On<NewPostPublishedMessage>("Test", message =>
        {
            Snackbar.Add($"New Post with slug: '{message.Slug}' created!");
            return Task.CompletedTask;
        });

        await hub.ConnectAsync();
    }
}