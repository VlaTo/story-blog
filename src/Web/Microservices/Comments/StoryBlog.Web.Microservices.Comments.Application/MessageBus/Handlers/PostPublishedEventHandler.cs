namespace StoryBlog.Web.Microservices.Comments.Application.MessageBus.Handlers;

/*public sealed class PostPublishedEventHandler : IConsumer<PostPublishedEvent>
{
    private readonly IAsyncUnitOfWork context;
    private readonly ILogger<NewPostCreatedMessageConsumer> logger;

    public PostPublishedEventHandler(
        IAsyncUnitOfWork context,
        ILogger<NewPostCreatedMessageConsumer> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public Task OnHandle(PostPublishedEvent message)
    {
        logger.LogInformation($"Received {nameof(PostPublishedEvent)} event");

        return Task.CompletedTask;
    }
}*/