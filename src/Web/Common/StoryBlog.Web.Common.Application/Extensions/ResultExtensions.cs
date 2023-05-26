namespace StoryBlog.Web.Common.Application.Extensions;

public static class ResultExtensions
{
    public static bool IsFailed<TValue>(this Result<TValue> result) => false == result.IsSuccess;
}