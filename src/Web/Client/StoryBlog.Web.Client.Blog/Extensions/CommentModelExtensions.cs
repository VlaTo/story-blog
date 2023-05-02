using StoryBlog.Web.Client.Blog.Core;
using StoryBlog.Web.Client.Blog.Models;

namespace StoryBlog.Web.Client.Blog.Extensions;

internal static class CommentModelExtensions
{
    public static CommentModel MapComment(
        this Microservices.Comments.Shared.Models.CommentModel source,
        CommentsCollection commentsCollection) =>
        new(source.Key, source.PostKey, source.ParentKey, source.Text, commentsCollection, source.CreatedAt);

    public static CommentModel MapReplace(this CommentModel source, CommentsCollection commentsCollection) =>
        new(source.Key, source.PostKey, source.ParentKey, source.Text, commentsCollection, source.CreateAt);
}