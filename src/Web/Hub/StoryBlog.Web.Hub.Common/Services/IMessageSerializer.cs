using StoryBlog.Web.Hub.Common.Messages;

namespace StoryBlog.Web.Hub.Common.Services;

public interface IMessageSerializer
{
    byte[] Serialize<TMessage>(TMessage message) where TMessage : IHubMessage;

    TMessage Deserialize<TMessage>(byte[] data) where TMessage : IHubMessage;
}