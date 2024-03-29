using StoryBlog.Web.Microservices.Comments.WebApi.MessageBus.Consumers;

namespace StoryBlog.Web.Microservices.Comments.WebApi.Extensions;

internal static class LoggerExtensions
{
    public static void LogCommentsForPostAlreadyExists(this ILogger<NewPostCreatedMessageConsumer> logger, Guid postKey)
    {
        //logger.LogWarning($"The \'{nameof(NewPostCreatedEvent)}\' event received, but comments for post: \"{postKey:D}\" already exists");
    }
}