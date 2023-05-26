using StoryBlog.Web.Common.Application.Extensions;

namespace StoryBlog.Web.Common.Application;

public class Result
{
    public static readonly Result Success;

    public bool IsSuccess
    {
        get;
        protected set;
    }

    public Exception? Exception
    {
        get;
        protected set;
    }

    public Result()
        : this(true, null)
    {
    }

    static Result()
    {
        Success = new Result();
    }

    public Result(Exception exception)
        : this(false, exception)
    {
    }

    protected Result(bool isSuccess, Exception? exception)
    {
        IsSuccess = isSuccess;
        Exception = exception;
    }

    public static implicit operator Result(Exception exception) => new(exception);
}

public sealed class Result<TValue> : Result
{
    public TValue? Value
    {
        get;
    }

    public Result(TValue value)
        : base(true, null)
    {
        Value = value;
    }

    public Result(Exception exception)
        : base(false, exception)
    {
        Value = default;
    }

    public static implicit operator TValue(Result<TValue> result)
    {
        if (result.IsFailed())
        {
            throw result.Exception ?? new Exception();
        }

        return result.Value!;
    }

    public static implicit operator Result<TValue>(TValue value) => new(value);

    public static implicit operator Result<TValue>(Exception exception) => new(exception);
}