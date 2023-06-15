using Microsoft.AspNetCore.WebUtilities;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;

namespace StoryBlog.Web.Microservices.Identity.Application.Extensions;

public static class StringExtensions
{
    private const string SlashStr = "/";
    
    public static string? GetOrigin(this string? url)
    {
        if (null != url)
        {
            Uri uri;

            try
            {
                uri = new Uri(url);
            }
            catch (Exception)
            {
                return null;
            }

            return $"{uri.Scheme}://{uri.Authority}";
        }

        return null;
    }

    [DebuggerStepThrough]
    public static string? EnsureLeadingSlash(this string? url)
    {
        return url != null && !url.StartsWith(SlashStr) ? SlashStr + url : url;
    }

    [DebuggerStepThrough]
    public static string? EnsureTrailingSlash(this string? url)
    {
        return url != null && !url.EndsWith(SlashStr) ? url + SlashStr : url;
    }

    [DebuggerStepThrough]
    public static string? RemoveLeadingSlash(this string? url)
    {
        if (url != null && url.StartsWith(UriSymbols.Slash))
        {
            url = url.Substring(1);
        }

        return url;
    }

    [DebuggerStepThrough]
    public static string? RemoveTrailingSlash(this string? url)
    {
        if (url != null && url.EndsWith(UriSymbols.Slash))
        {
            url = url.Substring(0, url.Length - 1);
        }

        return url;
    }

    [DebuggerStepThrough]
    public static IEnumerable<string> FromSpaceSeparatedString(this string input)
    {
        return input
            .Trim()
            .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
            .ToList();
    }

    [DebuggerStepThrough]
    public static NameValueCollection ReadQueryStringAsNameValueCollection(this string? url)
    {
        if (null != url)
        {
            var idx = url.IndexOf(UriSymbols.QuestionMark);

            if (idx >= 0)
            {
                url = url.Substring(idx + 1);
            }
            
            var query = QueryHelpers.ParseNullableQuery(url);
            
            if (null != query)
            {
                return query.AsNameValueCollection();
            }
        }

        return new NameValueCollection();
    }

    [DebuggerStepThrough]
    public static bool IsPresent(this string? value)
    {
        return !String.IsNullOrWhiteSpace(value);
    }
    
    [DebuggerStepThrough]
    public static bool IsMissing(this string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    [DebuggerStepThrough]
    public static bool IsLocalUrl(this string? url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return false;
        }

        // Allows "/" or "/foo" but not "//" or "/\".
        if (url[0] == UriSymbols.Slash)
        {
            // url is exactly "/"
            if (1 == url.Length)
            {
                return true;
            }

            // url doesn't start with "//" or "/\"
            if (url[1] != UriSymbols.Slash && url[1] != UriSymbols.BackSlash)
            {
                return true;
            }

            return false;
        }

        // Allows "~/" or "~/foo" but not "~//" or "~/\".
        if (url[0] == UriSymbols.Tilde && url.Length > 1 && url[1] == UriSymbols.Slash)
        {
            // url is exactly "~/"
            if (2 == url.Length)
            {
                return true;
            }

            // url doesn't start with "~//" or "~/\"
            if (url[2] != UriSymbols.Slash && url[2] != UriSymbols.BackSlash)
            {
                return true;
            }

            return false;
        }

        return false;
    }

    [DebuggerStepThrough]
    public static string CleanUrlPath(this string? url)
    {
        if (String.IsNullOrWhiteSpace(url))
        {
            url = SlashStr;
        }

        if ( url != SlashStr && url.EndsWith(SlashStr))
        {
            url = url.Substring(0, url.Length - 1);
        }

        return url;
    }

    [DebuggerStepThrough]
    public static string AddQueryString(this string url, string query)
    {
        if (false == url.Contains(UriSymbols.QuestionMark))
        {
            url += UriSymbols.QuestionMark;
        }
        else if (false == url.EndsWith(UriSymbols.Ampersand))
        {
            url += UriSymbols.Ampersand;
        }

        return url + query;
    }

    [DebuggerStepThrough]
    public static string AddQueryString(this string url, string name, string? value)
    {
        var encoder = UrlEncoder.Default;
        return null != value
            ? url.AddQueryString(encoder.Encode(name) + UriSymbols.EqualsMark + encoder.Encode(value))
            : url;
    }

    [DebuggerStepThrough]
    public static string AddHashFragment(this string url, string query)
    {
        if (false == url.Contains(UriSymbols.HashMark))
        {
            url += UriSymbols.HashMark;
        }

        return url + query;
    }

    public static List<string> ParseScopesString(this string? scopes)
    {
        var parsedScopes = new List<string>();

        if (false == scopes.IsMissing())
        {
            parsedScopes.AddRange(scopes
                .Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
            );

            if (0 < parsedScopes.Count)
            {
                parsedScopes.Sort();
            }
        }

        return parsedScopes;
    }

    /// <summary>
    /// Creates a SHA256 hash of the specified input.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns>A hash</returns>
    public static string Sha256(this string? input)
    {
        if (String.IsNullOrEmpty(input))
        {
            return String.Empty;
        }

        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = bytes.Sha256();

        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// Creates a SHA512 hash of the specified input.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns>A hash</returns>
    public static string Sha512(this string? input)
    {
        if (String.IsNullOrEmpty(input))
        {
            return String.Empty;
        }

        using (var algorithm = SHA512.Create())
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = algorithm.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }
    }

    public static string Obfuscate(this string value)
    {
        var last4Chars = "****";

        if (false == String.IsNullOrEmpty(value) && value.Length > 4)
        {
            last4Chars = value.Substring(value.Length - 4);
        }

        return "****" + last4Chars;
    }
}