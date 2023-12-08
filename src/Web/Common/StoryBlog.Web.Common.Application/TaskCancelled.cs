namespace StoryBlog.Web.Common.Application;

public sealed class TaskCancelled
{
    public static readonly TaskCancelled Instance = new();

    private TaskCancelled()
    {
    }
}