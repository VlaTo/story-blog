using Microsoft.AspNetCore.Http;

namespace StoryBlog.Web.Microservices.Identity.Application.Services.Defaults;

public sealed class DefaultHttpContextCancellationTokenProvider : ICancellationTokenProvider
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public CancellationToken CancellationToken => httpContextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;

    public DefaultHttpContextCancellationTokenProvider(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }
}