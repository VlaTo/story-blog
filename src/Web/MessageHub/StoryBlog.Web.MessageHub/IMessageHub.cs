using StoryBlog.Web.MessageHub.Messages;

namespace StoryBlog.Web.MessageHub;

/// <summary>
/// 
/// </summary>
public interface IMessageHub
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <param name="channel"></param>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task PublishAsync<TMessage>(string channel, TMessage message, CancellationToken cancellationToken = default)
        where TMessage : IHubMessage;
}