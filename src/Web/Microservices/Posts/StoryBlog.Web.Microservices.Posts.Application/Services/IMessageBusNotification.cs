using StoryBlog.Web.Microservices.Posts.Application.Models;

namespace StoryBlog.Web.Microservices.Posts.Application.Services;

public interface IMessageBusNotification
{
    Task NewPostCreatedAsync(NewPostCreated postCreated, CancellationToken cancellationToken);

    Task PostDeletedAsync(Guid postKey, string authorId, CancellationToken cancellationToken);

    Task PublishPostProcessedAsync(Guid postKey, CancellationToken cancellationToken);
}