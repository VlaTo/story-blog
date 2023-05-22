using Microsoft.Extensions.DependencyInjection;

namespace StoryBlog.Web.Microservices.Identity.Application.DependencyInjection;

public interface IIdentityServerBuilder
{
    IServiceCollection Services
    {
        get;
    }
}