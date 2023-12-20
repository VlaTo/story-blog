using StoryBlog.Web.Client.Blog.Core;

namespace StoryBlog.Web.Client.Blog.Models;

public class CommentModel : ICommentBase
{
    public Guid Key
    {
        get;
    }

    public Guid PostKey
    {
        get;
    }

    public Guid? ParentKey
    {
        get;
    }

    public string Text
    {
        get;
    }

    public string? AuthorId
    {
        get;
    }

    public CommentsCollection Comments
    {
        get;
    }

    public DateTimeOffset CreateAt
    {
        get;
    }

    public CommentModel(
        Guid key,
        Guid postKey,
        Guid? parentKey,
        string text,
        string? authorId,
        CommentsCollection comments,
        DateTimeOffset createAt)
    {
        Key = key;
        PostKey = postKey;
        ParentKey = parentKey;
        Text = text;
        AuthorId = authorId;
        Comments = comments;
        CreateAt = createAt;
    }
}