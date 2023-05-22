using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;

namespace StoryBlog.Web.Client.Blog.Shared;

public partial class LoginDisplay
{
    [Inject]
    private NavigationManager NavigationManager
    {
        get;
        set;
    }

    [Inject]
    private IOptionsSnapshot<RemoteAuthenticationOptions<OidcProviderOptions>> OptionsSnapshot
    {
        get;
        set;
    }

    private void BeginLogOut(MouseEventArgs _)
    {
        var options = OptionsSnapshot.Get(Options.DefaultName);
        var paths = options.AuthenticationPaths;

        NavigationManager.NavigateToLogout(paths.LogOutPath);
    }
}