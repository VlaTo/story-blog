using Microsoft.Extensions.DependencyInjection;

namespace StoryBlog.Web.Microservices.Identity.Application.DependencyInjection;

public sealed class IdentityServerBuilder : IIdentityServerBuilder
{
    public IServiceCollection Services
    {
        get;
    }

    public IdentityServerBuilder(IServiceCollection services)
    {
        Services = services;
    }
}