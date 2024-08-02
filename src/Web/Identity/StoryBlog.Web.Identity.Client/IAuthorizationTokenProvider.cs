namespace StoryBlog.Web.Identity.Client;

public interface IAuthorizationTokenProvider
{
    Task<AuthorizationToken?> GetAuthorizationTokenAsync(CancellationToken cancellationToken);
}