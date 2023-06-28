using Microsoft.AspNetCore.Components;
using MudBlazor;
using StoryBlog.Web.MessageHub.Client;
using StoryBlog.Web.Microservices.Posts.Shared.Messages;

namespace StoryBlog.Web.Client.Blog.Shared;

public partial class MainLayout
{
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

        var connection = new MessageHubConnectionBuilder()
            .WithUrl("ws://localhost:5033/notification")
            .Build();

        connection
            .On<NewPostPublishedMessage>("Test", async message =>
            {
                Snackbar.Add("New Blog Post created!");
                await Task.CompletedTask;
            });

        await connection.ConnectAsync();
    }
}