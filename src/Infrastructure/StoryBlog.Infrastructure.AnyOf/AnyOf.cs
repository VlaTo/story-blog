namespace StoryBlog.Infrastructure.AnyOf;

/// <summary>
/// 
/// </summary>
public class AnyOf
{
    protected int Index
    {
        get;
    }

    protected AnyOf(int index)
    {
        Index = index;
    }
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
public class AnyOf<T1, T2> : AnyOf
{
    protected T1? Item1
    {
        get;
    }

    protected T2? Item2
    {
        get;
    }

    public bool IsOfT1 => 0 == Index;

    public bool IsOfT2 => 1 == Index;

    public T1 Value1
    {
        get
        {
            if (false == IsOfT1)
            {
                throw new Exception();
            }

            return Item1!;
        }
    }

    public T2 Value2
    {
        get
        {
            if (false == IsOfT2)
            {
                throw new Exception();
            }

            return Item2!;
        }
    }

    protected AnyOf(int index, T1? item1, T2? item2)
        : base(index)
    {
        Item1 = item1;
        Item2 = item2;
    }

    public AnyOf(T1? item1)
        : this(0, item1, default)
    {
    }

    public AnyOf(T2? item2)
        : this(1, default, item2)
    {
    }

    public TResult Match<TResult>(Func<T1, TResult> action1, Func<T2, TResult> action2) => Index switch
    {
        0 => action1.Invoke(Value1),
        1 => action2.Invoke(Value2),
        _ => throw new NotSupportedException()
    };

    public static implicit operator AnyOf<T1, T2>(T1 value) => new(value);

    public static implicit operator AnyOf<T1, T2>(T2 value) => new(value);
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
public class AnyOf<T1, T2, T3> : AnyOf<T1, T2>
{
    protected T3? Item3
    {
        get;
    }

    public bool IsOfT3 => 2 == Index;

    public T3 Value3
    {
        get
        {
            if (false == IsOfT2)
            {
                throw new Exception();
            }

            return Item3!;
        }
    }

    private AnyOf(int index, T1? item1, T2? item2, T3? item3)
        : base(index, item1, item2)
    {
        Item3 = item3;
    }

    public AnyOf(T1? item1)
        : this(0, item1, default, default)
    {
    }

    public AnyOf(T2? item2)
        : this(1, default, item2, default)
    {
    }

    public AnyOf(T3? item3)
        : this(2, default, default, item3)
    {
    }

    public TResult Match<TResult>(
        Func<T1, TResult> action1,
        Func<T2, TResult> action2,
        Func<T3, TResult> action3)
        => Index switch
        {
            0 => action1.Invoke(Value1),
            1 => action2.Invoke(Value2),
            2 => action3.Invoke(Value3),
            _ => throw new NotSupportedException()
        };

    public static implicit operator AnyOf<T1, T2, T3>(T1 value) => new(value);
    
    public static implicit operator AnyOf<T1, T2, T3>(T2 value) => new(value);
    
    public static implicit operator AnyOf<T1, T2, T3>(T3 value) => new(value);
}