using Microsoft.Extensions.Primitives;
using System.Collections.Specialized;
using System.Diagnostics;

namespace StoryBlog.Web.Microservices.Identity.Application.Extensions;

/// <summary>
/// 
/// </summary>
public static class ReadableStringCollectionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    public static NameValueCollection AsNameValueCollection(this IEnumerable<KeyValuePair<string, StringValues>> collection)
    {
        var nv = new NameValueCollection();

        foreach (var field in collection)
        {
            foreach (var val in field.Value)
            {
                // special check for some Azure product: https://github.com/DuendeSoftware/Support/issues/48
                if (!String.IsNullOrWhiteSpace(val))
                {
                    nv.Add(field.Key, val);
                }
            }
        }

        return nv;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    public static NameValueCollection AsNameValueCollection(this IDictionary<string, StringValues> collection)
    {
        var nv = new NameValueCollection();

        foreach (var field in collection)
        {
            foreach (var item in field.Value)
            {
                // special check for some Azure product: https://github.com/DuendeSoftware/Support/issues/48
                if (!String.IsNullOrWhiteSpace(item))
                {
                    nv.Add(field.Key, item);
                }
            }
        }

        return nv;
    }
}