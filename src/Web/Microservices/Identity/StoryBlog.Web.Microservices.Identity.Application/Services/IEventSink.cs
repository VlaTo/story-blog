using StoryBlog.Web.Microservices.Identity.Application.Core.Events;

namespace StoryBlog.Web.Microservices.Identity.Application.Services;

/// <summary>
/// Models persistence of events
/// </summary>
public interface IEventSink
{
    /// <summary>
    /// Raises the specified event.
    /// </summary>
    /// <param name="evt">The event.</param>
    Task PersistAsync(Event evt);
}