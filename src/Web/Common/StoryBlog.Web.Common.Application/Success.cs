namespace StoryBlog.Web.Common.Application;

public sealed class Success
{
    public static readonly Success Instance;

    private Success()
    {
    }

    static Success()
    {
        Instance = new Success();
    }
}