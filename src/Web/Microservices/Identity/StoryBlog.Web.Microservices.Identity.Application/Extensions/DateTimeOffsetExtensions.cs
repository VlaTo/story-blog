using System.Diagnostics;

namespace StoryBlog.Web.Microservices.Identity.Application.Extensions;

internal static class DateTimeOffsetExtensions
{
    [DebuggerStepThrough]
    public static TimeSpan GetLifetimeInSeconds(this DateTimeOffset creationTime, DateTimeOffset now)
    {
        return now - creationTime;
    }

    [DebuggerStepThrough]
    public static bool HasExceeded(this DateTimeOffset creationTime, TimeSpan timeout, DateTimeOffset now)
    {
        return now > (creationTime + timeout);
    }

    [DebuggerStepThrough]
    public static bool HasExpired(this DateTimeOffset? expirationTime, DateTimeOffset now)
    {
        return expirationTime.HasValue && expirationTime.Value.HasExpired(now);
    }

    [DebuggerStepThrough]
    public static bool HasExpired(this DateTimeOffset expirationTime, DateTimeOffset now)
    {
        return now > expirationTime;
    }
}