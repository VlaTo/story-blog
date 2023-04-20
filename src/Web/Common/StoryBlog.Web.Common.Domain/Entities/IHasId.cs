namespace StoryBlog.Web.Common.Domain.Entities;

public interface IHasId<out TId>
{
    TId Id
    {
        get;
    }
}