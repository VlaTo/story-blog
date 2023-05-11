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
        CommentsCollection comments,
        DateTime createAt,
        string correlationKey)
        : base(key, postKey, parentKey, text, comments, createAt)
    {
        CorrelationKey = correlationKey;
    }
}