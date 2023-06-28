using StoryBlog.Web.Hub.Common.Messages;

namespace StoryBlog.Web.MessageHub.Services;

/// <summary>
/// 
/// </summary>
public interface IHubMessageSerializer
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <param name="message"></param>
    /// <returns></returns>
    byte[] Serialize<TMessage>(TMessage message) where TMessage : IHubMessage;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="messageType"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    IHubMessage Deserialize(Type messageType, ArraySegment<byte> data);
}