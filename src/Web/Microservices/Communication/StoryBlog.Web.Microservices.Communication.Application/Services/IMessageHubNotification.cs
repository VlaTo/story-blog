using StoryBlog.Web.Microservices.Communication.Application.Models;

namespace StoryBlog.Web.Microservices.Communication.Application.Services;

public interface IMessageHubNotification
{
    Task PublishNewPostCreatedEventAsync(PublishNewPostCreated context, CancellationToken cancellationToken);
}