using Microsoft.Extensions.Logging;
using StoryBlog.Web.Hub.Common.Configuration;

namespace StoryBlog.Web.Hub.Services.Hosted;

internal sealed class MessageHubHostedService : HostedServiceBase
{
    private readonly MessageHubOptions options;
    private readonly ILogger<MessageHubHostedService> logger;

    public MessageHubHostedService(
        MessageHubOptions options,
        ILogger<MessageHubHostedService> logger)
        : base()
    {
        this.options = options;
        this.logger = logger;
    }
}