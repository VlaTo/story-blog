namespace StoryBlog.Web.Microservices.Posts.Application.Services;

public interface IMessageBusNotification
{
    Task NewPostCreatedAsync(Guid postKey, DateTimeOffset createdAt, string authorId, CancellationToken cancellationToken);

    Task PostDeletedAsync(Guid postKey, string authorId, CancellationToken cancellationToken);
}