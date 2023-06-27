using Microsoft.Extensions.Hosting;

namespace StoryBlog.Web.Hub.Services.Hosted;

internal abstract class HostedServiceBase : IHostedService
{
    protected HostedServiceBase()
    {
    }

    public virtual Task StartAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public virtual Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}