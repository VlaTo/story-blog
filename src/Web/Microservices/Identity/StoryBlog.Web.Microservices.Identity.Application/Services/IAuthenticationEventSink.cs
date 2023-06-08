using StoryBlog.Web.Microservices.Identity.Application.Core.Events;

namespace StoryBlog.Web.Microservices.Identity.Application.Services;

public interface IAuthenticationEventSink
{
    Task RaiseEventAsync(Event evt);
}