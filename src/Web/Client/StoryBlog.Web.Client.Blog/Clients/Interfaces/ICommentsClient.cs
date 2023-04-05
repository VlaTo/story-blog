using StoryBlog.Web.Microservices.Comments.Shared.Models;

namespace StoryBlog.Web.Client.Blog.Clients.Interfaces;

public interface ICommentsClient
{
    Task<CommentsListResponse?> GetCommentsAsync(Guid postKey);

    Task<CommentModel?> GetCommentAsync(Guid commentKey);
}