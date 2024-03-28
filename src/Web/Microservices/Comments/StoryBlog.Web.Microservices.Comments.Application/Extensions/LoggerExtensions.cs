using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Events;
using StoryBlog.Web.Microservices.Comments.Application.MessageBus.Handlers;

namespace StoryBlog.Web.Microservices.Comments.Application.Extensions;

internal static class LoggerExtensions
{
    public static void LogCommentsForPostAlreadyExists(this ILogger<NewPostCreatedMessageConsumer> logger, Guid postKey)
    {
        logger.LogWarning($"The \'{nameof(NewPostCreatedEvent)}\' event received, but comments for post: \"{postKey:D}\" already exists");
    }
}