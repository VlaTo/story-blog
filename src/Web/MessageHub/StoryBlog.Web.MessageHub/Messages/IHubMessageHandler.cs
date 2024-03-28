namespace StoryBlog.Web.MessageHub.Messages;

public interface IHubMessageHandler<in TMessage> where TMessage : IHubMessage
{
    Task HandleAsync(TMessage message, CancellationToken cancellationToken);
}