using SlimMessageBus;
using StoryBlog.Web.Microservices.Posts.Shared.Messages;

namespace StoryBlog.Web.Microservices.Communication.WebApi.MessageBus.Consumers;

public class PostDeletedMessageConsumer : MessageConsumer, IConsumer<PostDeletedMessage>
{
    protected PostDeletedMessageConsumer(ILogger logger)
        : base(logger)
    {
    }

    public Task OnHandle(PostDeletedMessage message)
    {

        return Task.CompletedTask;
    }
}