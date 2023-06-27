using StoryBlog.Web.Hub.Common.Messages;

namespace StoryBlog.Web.Hub.Services;

/// <summary>
/// 
/// </summary>
public interface IMessageHub
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SendAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
        where TMessage : IHubMessage;
}