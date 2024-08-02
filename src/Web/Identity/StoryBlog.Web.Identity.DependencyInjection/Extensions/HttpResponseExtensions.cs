using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace StoryBlog.Web.Identity.DependencyInjection.Extensions;

internal static class HttpResponseExtensions
{
    public static Task WriteJsonAsync<T>(this HttpResponse response, int statusCode, T obj)
    {
        response.StatusCode = statusCode;
        response.ContentType = MediaTypeNames.Application.Json;

        return JsonSerializer.SerializeAsync(response.Body, obj);
    }
}