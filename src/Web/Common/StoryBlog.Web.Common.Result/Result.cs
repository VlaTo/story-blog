using StoryBlog.Web.Common.Result.Extensions;

namespace StoryBlog.Web.Common.Result;

/// <summary>
/// Discriminating result object.
/// </summary>
public class Result : IResult
{
    public static readonly Result Success;

    /// <summary>
    /// 
    /// </summary>
    public bool Succeeded
    {
        get;
        private init;
    }

    /// <summary>
    /// 
    /// </summary>
    public Exception? Error
    {
        get;
        private init;
    }

    public Result()
        : this(true, null)
    {
    }

    public Result(Exception exception)
        : this(false, exception)
    {
    }

    protected Result(bool succeeded, Exception? error)
    {
        Succeeded = succeeded;
        Error = error;
    }

    static Result()
    {
        Success = new Result();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static IResult Fail() => new Result
    {
        Succeeded = false,
        Error = null
    };

    /// <summary>
    /// 
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static IResult Fail(Exception error) => new Result
    {
        Succeeded = false,
        Error = error
    };
    
    public static implicit operator Result(Exception exception) => new(exception);
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="TValue"></typeparam>
public class Result<TValue> : Result, IResult<TValue>
{
    /// <summary>
    /// 
    /// </summary>
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
        if (result.Failed())
        {
            throw result.Error ?? new Exception();
        }

        return result.Value!;
    }

    public static implicit operator Result<TValue>(TValue value) => new(value);

    public static implicit operator Result<TValue>(Exception exception) => new(exception);
}

/// <summary>
/// Discriminating result object of two generic types.
/// </summary>
public class Result<T1, T2> : Result
{
    protected short Index
    {
        get;
    }

    /// <summary>
    /// 
    /// </summary>
    public T1? Item1
    {
        get;
    }

    /// <summary>
    /// 
    /// </summary>
    public T2? Item2
    {
        get;
    }

    /// <summary>
    /// 
    /// </summary>
    public bool IsOfT1 => Succeeded && 0 == Index;

    /// <summary>
    /// 
    /// </summary>
    public bool IsOfT2 => Succeeded && 1 == Index;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public Result(T1 value)
        : this(true, null, 0, value, default)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public Result(T2 value)
        : this(true, null, 1, default, value)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="exception"></param>
    public Result(Exception exception)
        : this(false, exception, -1, default, default)
    {
    }

    protected Result(bool isSuccess, Exception? exception, short index, T1? item1, T2? item2)
        : base(isSuccess, exception)
    {
        Index = index;
        Item1 = item1;
        Item2 = item2;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="action1"></param>
    /// <param name="action2"></param>
    /// <param name="failed"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public TResult Select<TResult>(
        Func<T1, TResult> action1,
        Func<T2, TResult> action2,
        Func<Exception, TResult>? failed = null)
    {
        if (false == Succeeded)
        {
            if (null == failed)
            {
                throw new InvalidOperationException();
            }
        }

        return Index switch
        {
            0 => action1.Invoke(Item1!),
            1 => action2.Invoke(Item2!),
            _ => failed!.Invoke(Error!)
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="action1"></param>
    /// <param name="action2"></param>
    /// <param name="failed"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Do(
        Action<T1> action1,
        Action<T2> action2,
        Action<Exception>? failed = null)
    {
        if (false == Succeeded)
        {
            if (null == failed)
            {
                throw new InvalidOperationException();
            }
        }

        switch (Index)
        {
            case 0:
            {
                action1.Invoke(Item1!);
                return;
            }

            case 1:
            {
                action2.Invoke(Item2!);
                return;
            }

            default:
            {
                failed!.Invoke(Error!);
                break;
            }
        }
    }

    public static explicit operator T1(Result<T1, T2> result)
    {
        if (false == result.Succeeded)
        {
            throw result.Error!;
        }

        if (0 != result.Index)
        {
            throw new InvalidCastException();
        }

        return result.Item1!;
    }

    public static explicit operator T2(Result<T1, T2> result)
    {
        if (false == result.Succeeded)
        {
            throw result.Error!;
        }

        if (1 != result.Index)
        {
            throw new InvalidCastException();
        }

        return result.Item2!;
    }

    public static implicit operator Result<T1, T2>(T1 value) => new(value);

    public static implicit operator Result<T1, T2>(T2 value) => new(value);

    public static implicit operator Result<T1, T2>(Exception exception) => new(exception);
}

/// <summary>
/// Discriminating result object of tree generic types.
/// </summary>
public sealed class Result<T1, T2, T3> : Result<T1, T2>
{
    /// <summary>
    /// 
    /// </summary>
    public T3? Item3
    {
        get;
    }

    /// <summary>
    /// 
    /// </summary>
    public bool IsOfT3 => Succeeded && 2 == Index;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public Result(T1 value)
        : this(true, null, 0, value, default, default)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public Result(T2 value)
        : this(true, null, 1, default, value, default)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public Result(T3 value)
        : this(true, null, 2, default, default, value)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="exception"></param>
    public Result(Exception exception)
        : this(false, exception, -1, default, default, default)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isSuccess"></param>
    /// <param name="exception"></param>
    /// <param name="index"></param>
    /// <param name="item1"></param>
    /// <param name="item2"></param>
    /// <param name="item3"></param>
    private Result(bool isSuccess, Exception? exception, short index, T1? item1, T2? item2, T3? item3)
        : base(isSuccess, exception, index, item1, item2)
    {
        Item3 = item3;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="action1"></param>
    /// <param name="action2"></param>
    /// <param name="action3"></param>
    /// <param name="failed"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public TResult Select<TResult>(
        Func<T1, TResult> action1,
        Func<T2, TResult> action2,
        Func<T3, TResult> action3,
        Func<Exception, TResult>? failed = null)
    {
        if (false == Succeeded)
        {
            if (null == failed)
            {
                throw new InvalidOperationException();
            }
        }

        return Index switch
        {
            0 => action1.Invoke(Item1!),
            1 => action2.Invoke(Item2!),
            2 => action3.Invoke(Item3!),
            _ => failed!.Invoke(Error!)
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="action1"></param>
    /// <param name="action2"></param>
    /// <param name="action3"></param>
    /// <param name="failed"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Do(
        Action<T1> action1,
        Action<T2> action2,
        Action<T3> action3,
        Action<Exception>? failed = null)
    {
        if (false == Succeeded)
        {
            if (null == failed)
            {
                throw new InvalidOperationException();
            }
        }

        switch (Index)
        {
            case 0:
            {
                action1.Invoke(Item1!);
                return;
            }

            case 1:
            {
                action2.Invoke(Item2!);
                return;
            }

            case 2:
            {
                action3.Invoke(Item3!);
                return;
            }

            default:
            {
                failed!.Invoke(Error!);
                break;
            }
        }
    }

    public static explicit operator T1(Result<T1, T2, T3> result)
    {
        if (false == result.Succeeded)
        {
            throw result.Error!;
        }

        if (0 != result.Index)
        {
            throw new InvalidCastException();
        }

        return result.Item1!;
    }

    public static explicit operator T2(Result<T1, T2, T3> result)
    {
        if (false == result.Succeeded)
        {
            throw result.Error!;
        }

        if (1 != result.Index)
        {
            throw new InvalidCastException();
        }

        return result.Item2!;
    }

    public static explicit operator T3(Result<T1, T2, T3> result)
    {
        if (false == result.Succeeded)
        {
            throw result.Error!;
        }

        if (2 != result.Index)
        {
            throw new InvalidCastException();
        }

        return result.Item3!;
    }

    public static implicit operator Result<T1, T2, T3>(T1 value) => new(value);

    public static implicit operator Result<T1, T2, T3>(T2 value) => new(value);

    public static implicit operator Result<T1, T2, T3>(T3 value) => new(value);

    public static implicit operator Result<T1, T2, T3>(Exception exception) => new(exception);
}