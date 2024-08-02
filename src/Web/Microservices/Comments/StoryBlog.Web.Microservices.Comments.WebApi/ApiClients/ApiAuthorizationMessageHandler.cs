using StoryBlog.Web.Identity.Client;
using StoryBlog.Web.Identity.Client.Extensions;

namespace StoryBlog.Web.Microservices.Comments.WebApi.ApiClients;

public class ApiAuthorizationMessageHandler : DelegatingHandler
{
    private readonly IAuthorizationTokenProvider tokenProvider;

    public ApiAuthorizationMessageHandler(IAuthorizationTokenProvider tokenProvider)
    {
        this.tokenProvider = tokenProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var authorization = await tokenProvider.GetAuthorizationTokenAsync(cancellationToken);

        if (String.IsNullOrEmpty(authorization?.Token))
        {
            throw new Exception();
        }

        request.SetBearerToken(authorization.Token);

        return await base.SendAsync(request, cancellationToken);
    }
}