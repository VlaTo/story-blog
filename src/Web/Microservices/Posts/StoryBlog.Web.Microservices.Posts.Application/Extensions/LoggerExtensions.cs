using Microsoft.Extensions.Logging;
using StoryBlog.Web.Microservices.Posts.Application.Services;

namespace StoryBlog.Web.Microservices.Posts.Application.Extensions;

internal static class LoggerExtensions
{
    public static void LogBackgroundProcessingStarting(this ILogger logger)
    {
        logger.LogDebug("Start background processing");
    }

    public static void LogBackgroundProcessingReady(this ILogger logger)
    {
        logger.LogDebug("Waiting for background task...");
    }

    public static void LogBackgroundProcessingCompleted(this ILogger logger)
    {
        logger.LogDebug("Background processing completed");
    }

    public static void LogCreatingServicesScope(this ILogger logger, IBackgroundTask backgroundTask)
    {
        logger.LogDebug($"Creating services scope for task: '{backgroundTask.TaskKey:B}'");
    }

    public static void LogStartBackgroundTaskProcessing(this ILogger logger, IBackgroundTask backgroundTask)
    {
        logger.LogDebug($"Start processing task: '{backgroundTask.TaskKey:B}'");
    }

    public static void LogBackgroundTaskProcessed(this ILogger logger, IBackgroundTask backgroundTask)
    {
        logger.LogDebug($"Background task: '{backgroundTask.TaskKey:B}' processed");
    }

    public static void LogBackgroundTaskProcessingFailed(
        this ILogger logger,
        IBackgroundTask backgroundTask,
        Exception exception,
        System.Runtime.ExceptionServices.ExceptionDispatchInfo? exceptionInfo)
    {
        logger.LogError(exception, $"Failed processing task: '{backgroundTask.TaskKey:B}'");
    }

    public static void LogBackgroundTaskProcessingCompleted(this ILogger logger, IBackgroundTask backgroundTask)
    {
        logger.LogDebug($"Complete processing task: '{backgroundTask.TaskKey:B}'");
    }
}