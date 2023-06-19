using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace StoryBlog.Web.Microservices.Identity.Application.Extensions;

public static class StringEnumerableExtensions
{
    [return: NotNull]
    public static string ToSpaceSeparatedString(this IEnumerable<string>? strings)
    {
        return null == strings ? String.Empty : String.Join(' ', strings);
    }

    public static bool HasValue(this IEnumerable<string>? strings) => null != strings && strings.Any();

    public static bool NoValue(this IEnumerable<string>? strings) => null == strings || false == strings.Any();

    internal static bool AreValidResourceIndicatorFormat(this IEnumerable<string>? list, ILogger logger)
    {
        if (null != list)
        {
            foreach (var item in list)
            {
                if (!Uri.IsWellFormedUriString(item, UriKind.Absolute))
                {
                    logger.LogDebug("Resource indicator {resource} is not a valid URI.", item);
                    return false;
                }

                if (item.Contains("#"))
                {
                    logger.LogDebug("Resource indicator {resource} must not contain a fragment component.", item);
                    return false;
                }
            }
        }

        return true;
    }
}