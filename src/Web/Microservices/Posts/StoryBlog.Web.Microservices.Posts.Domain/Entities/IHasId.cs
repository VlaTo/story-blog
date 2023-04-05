namespace StoryBlog.Web.Microservices.Posts.Domain.Entities;

public interface IHasId<out TId>
{
    TId Id
    {
        get;
    }
}