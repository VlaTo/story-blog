using StoryBlog.Web.Hub.Common.Messages;

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
    Task SendAsync<TMessage>(string channel, TMessage message, CancellationToken cancellationToken = default)
        where TMessage : IHubMessage;
}