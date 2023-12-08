namespace StoryBlog.Web.Common.Application;

public sealed class NotFound
{
    public static readonly NotFound Instance;

    private NotFound()
    {
    }

    static NotFound()
    {
        Instance = new NotFound();
    }
}