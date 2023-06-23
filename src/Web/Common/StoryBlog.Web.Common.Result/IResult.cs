namespace StoryBlog.Web.Common.Result;

/// <summary>
/// 
/// </summary>
public interface IResult
{
    /// <summary>
    /// 
    /// </summary>
    bool Succeeded
    {
        get;
    }

    /// <summary>
    /// 
    /// </summary>
    Exception? Error
    {
        get;
    }
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="TValue"></typeparam>
public interface IResult<out TValue> : IResult
{
    /// <summary>
    /// 
    /// </summary>
    TValue Value
    {
        get;
    }
}