namespace StoryBlog.Web.Identity.Core;

public class Result : IResult
{
    public bool Succeeded
    {
        get;
        private protected init;
    }

    public string? Error
    {
        get;
        private protected init;
    }

    public static IResult Success() => new Result
    {
        Succeeded = true,
        Error = null
    };

    public static Task<IResult> SuccessAsync => Task.FromResult(Success());

    public static IResult Fail() => new Result
    {
        Succeeded = false,
        Error = null
    };

    public static IResult Fail(string error) => new Result
    {
        Succeeded = false,
        Error = error
    };
    
    public static Task<IResult> FailAsync() => Task.FromResult(Fail());

    public static Task<IResult> FailAsync(string error) => Task.FromResult(Fail(error));
}

public class Result<T> : Result, IResult<T>
{
    public T Data
    {
        get;
        private protected init;
    }

    public static IResult<T> Success(T data) => new Result<T>
    {
        Succeeded = true,
        Error = null,
        Data = data
    };

    public static Task<IResult<T>> SuccessAsync(T data) => Task.FromResult(Success(data));

    public static IResult<T> Fail() => new Result<T>
    {
        Succeeded = false,
        Error = null,
        Data = default!
    };

    public static IResult<T> Fail(string error) => new Result<T>
    {
        Succeeded = false,
        Error = error,
        Data = default!
    };
    
    public static Task<IResult<T>> FailAsync() => Task.FromResult(Fail());

    public static Task<IResult<T>> FailAsync(string error) => Task.FromResult(Fail(error));
}