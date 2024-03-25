using Microsoft.AspNetCore.Mvc;

namespace TestProject1.Extensions;

internal static class ActionResultExtensions
{
    public static T AsResult<T>(this IActionResult result) where T : notnull
    {
        return (T)result;
    }
}