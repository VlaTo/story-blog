using StoryBlog.Web.Client.Blog.Core;

namespace StoryBlog.Web.Client.Blog.Models;

public sealed class NewCommentModel : CommentModel
{
    public string CorrelationKey
    {
        get;
    }

    public NewCommentModel(
        Guid key,
        Guid postKey,
        Guid? parentKey,
        string text,
        string? authorId,
        CommentsCollection comments,
        DateTimeOffset createAt,
        string correlationKey)
        : base(key, postKey, parentKey, text, authorId, comments, createAt)
    {
        CorrelationKey = correlationKey;
    }
}