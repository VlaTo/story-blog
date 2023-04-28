using StoryBlog.Web.Microservices.Comments.Shared.Models;

namespace StoryBlog.Web.Client.Blog.Clients.Interfaces;

public interface ICommentsClient
{
    Task<ListAllResponse?> GetRootCommentsAsync(Guid postKey, int pageNumber, int pageSize);

    Task<IReadOnlyList<CommentModel>?> GetChildCommentsAsync(Guid postKey, Guid parentKey);

    Task<CommentModel?> GetCommentAsync(Guid commentKey);

    Task<CreatedCommentModel?> CreateCommentAsync(Guid postKey, Guid? parentKey, string text);
}