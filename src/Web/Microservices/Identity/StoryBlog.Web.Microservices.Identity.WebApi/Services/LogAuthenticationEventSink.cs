using StoryBlog.Web.Microservices.Identity.Application.Core.Events;
using StoryBlog.Web.Microservices.Identity.Application.Services;

namespace StoryBlog.Web.Microservices.Identity.WebApi.Services;

internal sealed class LogAuthenticationEventSink : IAuthenticationEventSink
{
    private readonly ILogger<LogAuthenticationEventSink> logger;

    public LogAuthenticationEventSink(ILogger<LogAuthenticationEventSink> logger)
    {
        this.logger = logger;
    }

    public async Task RaiseEventAsync(Event evt)
    {
        using var stream = new MemoryStream();
        await System.Text.Json.JsonSerializer.SerializeAsync(stream, evt);
    }
}