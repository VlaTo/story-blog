namespace TestProject1.Extensions;

internal static class ObjectExtensions
{
    public static T? As<T>(this object? value) where T : class
    {
        return (T?)value;
    }
}