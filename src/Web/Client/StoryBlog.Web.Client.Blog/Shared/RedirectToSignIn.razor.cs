using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;

namespace StoryBlog.Web.Client.Blog.Shared;

public partial class RedirectToSignIn
{
    [Inject]
    private NavigationManager NavigationManager
    {
        get;
        set;
    }

    [Inject]
    //private IOptionsSnapshot<RemoteAuthenticationOptions<ApiAuthorizationProviderOptions>> OptionsSnapshot
    private IOptionsSnapshot<RemoteAuthenticationOptions<OidcProviderOptions>> OptionsSnapshot
    {
        get;
        set;
    }

    protected override void OnInitialized()
    {
        var options = OptionsSnapshot.Get(Options.DefaultName);
        var paths = options.AuthenticationPaths;

        NavigationManager.NavigateToLogin(paths.LogInPath);
    }
}