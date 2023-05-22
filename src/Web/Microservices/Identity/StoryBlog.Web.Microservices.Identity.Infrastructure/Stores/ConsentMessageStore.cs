using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Models.Messages;
using StoryBlog.Web.Microservices.Identity.Application.Models.Responses;
using StoryBlog.Web.Microservices.Identity.Application.Stores;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Stores;

/// <summary>
/// 
/// </summary>
public class ConsentMessageStore : IConsentMessageStore
{
    protected readonly MessageCookie<ConsentResponse> Cookie;

    public ConsentMessageStore(MessageCookie<ConsentResponse> cookie)
    {
        Cookie = cookie;
    }

    public virtual Task DeleteAsync(string id)
    {
        using var activity = Tracing.ActivitySource.StartActivity("ConsentMessageStore.Delete");

        Cookie.Clear(id);
        return Task.CompletedTask;
    }

    public virtual Task<Message<ConsentResponse>?> ReadAsync(string id)
    {
        using var activity = Tracing.ActivitySource.StartActivity("ConsentMessageStore.Read");

        var message = Cookie.Read(id);

        return Task.FromResult(message);
    }

    public virtual Task WriteAsync(string id, Message<ConsentResponse> message)
    {
        using var activity = Tracing.ActivitySource.StartActivity("ConsentMessageStore.Write");

        Cookie.Write(id, message);

        return Task.CompletedTask;
    }
}