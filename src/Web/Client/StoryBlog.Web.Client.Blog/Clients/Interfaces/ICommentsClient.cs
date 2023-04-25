using StoryBlog.Web.Microservices.Comments.Shared.Models;
using CreatedCommentModel = StoryBlog.Web.Client.Blog.Models.CreatedCommentModel;

namespace StoryBlog.Web.Client.Blog.Clients.Interfaces;

public interface ICommentsClient
{
    Task<ListAllResponse?> GetCommentsAsync(Guid postKey);

    Task<CommentModel?> GetCommentAsync(Guid commentKey);

    Task<CreatedCommentModel?> CreateCommentAsync(Guid postKey, Guid? parentKey, string text);
}