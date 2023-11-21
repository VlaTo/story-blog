namespace StoryBlog.Web.Common.Domain.Entities;

public interface IHasUserId<out TId>
{
    TId UserId { get; }
}