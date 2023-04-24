using StoryBlog.Web.Microservices.Comments.Shared.Models;

namespace StoryBlog.Web.Client.Blog.Clients.Interfaces;

public interface ICommentsClient
{
    Task<ListAllResponse?> GetCommentsAsync(Guid postKey);

    Task<CommentModel?> GetCommentAsync(Guid commentKey);

    Task CreateCommentAsync(Guid postKey, Guid? parentKey, string text, DateTime dateTime);
}