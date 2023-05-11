using System.Text.Encodings.Web;
using System.Text;

namespace StoryBlog.Web.Client.Blog.Extensions;

internal static class QueryHelpers
{
    public static string AddQueryString(string uri, IDictionary<string, string?> queryString)
    {
        return AddQueryString(uri, queryString.AsEnumerable());
    }

    public static string AddQueryString(string uri, IEnumerable<KeyValuePair<string, string?>> queryString)
    {
        var anchorIndex = uri.IndexOf('#');
        var uriToBeAppended = uri.AsSpan();
        var anchorText = ReadOnlySpan<char>.Empty;
        
        // If there is an anchor, then the query string must be inserted before its first occurrence.
        if (-1 < anchorIndex)
        {
            anchorText = uriToBeAppended.Slice(anchorIndex);
            uriToBeAppended = uriToBeAppended.Slice(0, anchorIndex);
        }

        var queryIndex = uriToBeAppended.IndexOf('?');
        var hasQuery = queryIndex != -1;

        var sb = new StringBuilder();

        sb.Append(uriToBeAppended);
        
        foreach (var kvp in queryString)
        {
            if (null == kvp.Value)
            {
                continue;
            }

            sb.Append(hasQuery ? '&' : '?');
            sb.Append(UrlEncoder.Default.Encode(kvp.Key));
            sb.Append('=');
            sb.Append(UrlEncoder.Default.Encode(kvp.Value));
            
            hasQuery = true;
        }

        sb.Append(anchorText);
        
        return sb.ToString();
    }
}