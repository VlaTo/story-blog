namespace StoryBlog.Web.Client.Blog.Models;

public sealed class EmptyList
{
    public static readonly EmptyList Instance;

    private EmptyList()
    {
    }

    static EmptyList()
    {
        Instance = new EmptyList();
    }
}