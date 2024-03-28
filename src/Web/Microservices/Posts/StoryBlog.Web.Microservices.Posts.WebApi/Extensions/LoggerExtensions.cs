namespace StoryBlog.Web.Microservices.Posts.WebApi.Extensions;

internal static class LoggerExtensions
{
    public static void LogPostCreateNotificationSent(this ILogger logger, Guid postKey, DateTimeOffset created, string authorId)
    {
        logger.LogDebug($"Notification for new post: {postKey} was sent");
    }

    public static void LogPostCreatedNotificationDisabled(this ILogger logger, Guid postKey, string authorId)
    {
        logger.LogWarning("Notification for new post disabled");
    }

    public static void LogPostDeletedNotificationSent(this ILogger logger, Guid postKey, string authorId)
    {
        logger.LogDebug($"Notification for deleted post: {postKey} was sent");
    }

    public static void LogPostDeletedNotificationDisabled(this ILogger logger, Guid postKey)
    {
        logger.LogWarning("Notification for deleted post disabled");
    }
}