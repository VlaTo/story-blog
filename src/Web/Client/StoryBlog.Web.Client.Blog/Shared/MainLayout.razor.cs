using MudBlazor;

namespace StoryBlog.Web.Client.Blog.Shared;

public partial class MainLayout
{
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
}