using Microsoft.AspNetCore.Authentication;

namespace StoryBlog.Web.Microservices.Identity.Application.Extensions;

public static class SystemClockExtensions
{
    internal static TimeSpan GetAge(this ISystemClock clock, DateTime date)
    {
        var now = clock.UtcNow.UtcDateTime;

        if (date > now)
        {
            now = date;
        }

        return now.Subtract(date);
    }
}