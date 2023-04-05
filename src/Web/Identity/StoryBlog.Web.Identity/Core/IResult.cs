namespace StoryBlog.Web.Identity.Core;

public interface IResult
{
    bool Succeeded
    {
        get;
    }

    string? Error
    {
        get;
    }
}

public interface IResult<T> : IResult
{
    T Data
    {
        get;
    }
}