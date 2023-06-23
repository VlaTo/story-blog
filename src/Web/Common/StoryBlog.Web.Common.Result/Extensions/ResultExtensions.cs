namespace StoryBlog.Web.Common.Result.Extensions;

/// <summary>
/// 
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool Failed<TValue>(this Result<TValue> result) => false == result.Succeeded;
    
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool Failed<T1, T2>(this Result<T1, T2> result) => false == result.Succeeded;
    
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool Failed<T1, T2, T3>(this Result<T1, T2, T3> result) => false == result.Succeeded;
}