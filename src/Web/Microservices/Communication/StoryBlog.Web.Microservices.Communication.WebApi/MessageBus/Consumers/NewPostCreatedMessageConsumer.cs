using SlimMessageBus;
using StoryBlog.Web.MessageHub;
using StoryBlog.Web.Microservices.Communication.Shared.Messages;
using StoryBlog.Web.Microservices.Posts.Shared.Messages;

namespace StoryBlog.Web.Microservices.Communication.WebApi.MessageBus.Consumers;

public class NewPostCreatedMessageConsumer : MessageConsumer, IConsumer<NewPostCreatedMessage>
{
    private readonly IMessageHub messageHub;

    public NewPostCreatedMessageConsumer(
        IMessageHub messageHub,
        ILogger<NewPostCreatedMessageConsumer> logger)
        : base(logger)
    {
        this.messageHub = messageHub;
    }

    public async Task OnHandle(NewPostCreatedMessage message)
    {
        await messageHub.SendAsync("post.created", new NewPostPublishedMessage(message.PostKey, "test"));
        Logger.LogError("StoryBlog.Web.Microservices.Communication.WebApi.MessageBus.Consumers.NewPostCreatedMessageConsumer");
    }
}