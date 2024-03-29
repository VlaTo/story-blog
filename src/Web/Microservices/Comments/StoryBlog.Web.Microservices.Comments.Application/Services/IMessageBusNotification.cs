using StoryBlog.Web.Microservices.Comments.Application.Models;

namespace StoryBlog.Web.Microservices.Comments.Application.Services;

public interface IMessageBusNotification
{
    Task NewCommentCreatedAsync(NewCommentCreated commentCreated, CancellationToken cancellationToken);
}