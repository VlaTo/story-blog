using Microsoft.Extensions.Logging;
using StoryBlog.Web.Microservices.Posts.Application.Handlers.CreatePost;

namespace StoryBlog.Web.Microservices.Posts.Application.Extensions;

internal static class CreatePostHandlerLoggerExtensions
{
    public static ILogger<CreatePostHandler> LogEntityCreated(this ILogger<CreatePostHandler> logger)
    {
        logger.LogDebug($"Entity {nameof(Domain.Entities.Post)} created");

        return logger;
    }
}