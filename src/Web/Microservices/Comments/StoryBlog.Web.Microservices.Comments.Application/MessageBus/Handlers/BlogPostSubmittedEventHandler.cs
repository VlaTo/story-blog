using Microsoft.Extensions.Logging;
using SlimMessageBus;
using StoryBlog.Web.Common.Events;

namespace StoryBlog.Web.Microservices.Comments.Application.MessageBus.Handlers;

public sealed class BlogPostSubmittedEventHandler : IConsumer<BlogPostEvent>
{
    private readonly ILogger<BlogPostSubmittedEventHandler> logger;

    public BlogPostSubmittedEventHandler(ILogger<BlogPostSubmittedEventHandler> logger)
    {
        this.logger = logger;
    }

    public Task OnHandle(BlogPostEvent message)
    {
        logger.LogInformation($"Received event: action = {message.Action} for key: \"{message.Key}\"");
        return Task.CompletedTask;
    }
}