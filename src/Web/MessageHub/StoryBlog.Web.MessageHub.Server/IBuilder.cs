namespace StoryBlog.Web.MessageHub.Server;

public interface IBuilder<out T>
{
    T Build();
}