using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using StoryBlog.Web.Microservices.Identity.Application.Configuration;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace StoryBlog.Web.Microservices.Identity.Application.Extensions;

internal static class HttpResponseExtensions
{
    public static void AddScriptCspHeaders(this HttpResponse response, CspOptions options, string hash)
    {
        var csp1Part = options.Level == CspLevel.One ? "'unsafe-inline' " : string.Empty;
        var cspHeader = $"default-src 'none'; script-src {csp1Part}'{hash}'";

        AddCspHeaders(response.Headers, options, cspHeader);
    }
    
    public static void AddCspHeaders(IHeaderDictionary headers, CspOptions options, string cspHeader)
    {
        if (false == headers.ContainsKey(HeaderNames.ContentSecurityPolicy))
        {
            headers.Append(HeaderNames.ContentSecurityPolicy, cspHeader);
        }

        if (options.AddDeprecatedHeader && false == headers.ContainsKey(Core.HeaderNames.XContentSecurityPolicy))
        {
            headers.Append(Core.HeaderNames.XContentSecurityPolicy, cspHeader);
        }
    }

    public static void SetNoCache(this HttpResponse response)
    {
        var noCache = new CacheControlHeaderValue
            {
                NoStore = true,
                NoCache = true,
                MaxAge = TimeSpan.Zero
            }
            .ToString();

        /*if (false == response.Headers.ContainsKey(HeaderNames.CacheControl))
        {

            response.Headers.Add("Cache-Control", "no-store, no-cache, max-age=0");
        }
        else
        {
            response.Headers["Cache-Control"] = "no-store, no-cache, max-age=0";
        }*/

        response.Headers[HeaderNames.CacheControl] = new StringValues(noCache);

        if (false == response.Headers.ContainsKey(HeaderNames.Pragma))
        {
            response.Headers.Append(HeaderNames.Pragma, CacheControlHeaderValue.NoCacheString);
        }
    }

    public static void SetCache(this HttpResponse response, int maxAge, params string[]? varyBy)
    {
        if (0 == maxAge)
        {
            SetNoCache(response);
        }
        else if (0 < maxAge)
        {
            if (false == response.Headers.ContainsKey(HeaderNames.CacheControl))
            {
                var cacheControl = new CacheControlHeaderValue
                {
                    MaxAge = TimeSpan.FromSeconds(maxAge)
                };
                response.Headers.Append(HeaderNames.CacheControl, new StringValues(cacheControl.ToString()));
            }

            if (true == varyBy?.Any())
            {
                var vary = new StringValues(varyBy);

                if (response.Headers.ContainsKey(HeaderNames.Vary))
                {
                    vary = StringValues.Concat(response.Headers.Vary, vary);
                }

                response.Headers[HeaderNames.Vary] = vary;
            }
        }
    }

    public static Task WriteJsonAsync<T>(this HttpResponse response, int statusCode, T obj)
    {
        response.StatusCode = statusCode;
        response.ContentType = MediaTypeNames.Application.Json;

        return JsonSerializer.SerializeAsync(response.Body, obj);
    }

    public static async Task WriteJsonAsync(this HttpResponse response, object obj, string? contentType = null)
    {
        var encoding = Encoding.UTF8;
        var json = Core.ObjectSerializer.ToString(obj);
        var ct = new ContentType(contentType ?? MediaTypeNames.Application.Json)
        {
            CharSet = encoding.HeaderName
        };

        response.ContentType = ct.ToString();

        await response.WriteAsync(json, encoding);
        await response.Body.FlushAsync();
    }

    public static async Task WriteHtmlAsync(this HttpResponse response, string html)
    {
        var encoding = Encoding.UTF8;
        var contentType = new ContentType(MediaTypeNames.Text.Html)
        {
            CharSet = encoding.HeaderName
        };

        response.ContentType = contentType.ToString();

        await response.WriteAsync(html, encoding);
        await response.Body.FlushAsync();
    }
}