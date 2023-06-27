using StoryBlog.Web.Hub.Common.Messages;
using StoryBlog.Web.Microservices.Posts.Shared.Messages;

namespace StoryBlog.Web.Client.Blog.Core.MessageHandlers;

internal sealed class NewPostPublishedMessageHandler : IHubMessageHandler<NewPostPublishedMessage>
{
    public NewPostPublishedMessageHandler()
    {
    }

    public Task HandleAsync(NewPostPublishedMessage message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}