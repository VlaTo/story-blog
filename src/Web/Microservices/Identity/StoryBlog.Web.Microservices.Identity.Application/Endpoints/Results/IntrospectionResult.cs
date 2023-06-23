using Microsoft.AspNetCore.Http;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Application.Hosting;

namespace StoryBlog.Web.Microservices.Identity.Application.Endpoints.Results;

/// <summary>
/// Result for introspection
/// </summary>
/// <seealso cref="IEndpointResult" />
internal sealed class IntrospectionResult : IEndpointResult
{
    /// <summary>
    /// Gets the result.
    /// </summary>
    /// <value>
    /// The result.
    /// </value>
    public Dictionary<string, object> Entries
    {
        get;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IntrospectionResult"/> class.
    /// </summary>
    /// <param name="entries">The result.</param>
    /// <exception cref="System.ArgumentNullException">result</exception>
    public IntrospectionResult(Dictionary<string, object> entries)
    {
        Entries = entries ?? throw new ArgumentNullException(nameof(entries));
    }

    /// <summary>
    /// Executes the result.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns></returns>
    public Task ExecuteAsync(HttpContext context)
    {
        context.Response.SetNoCache();

        return context.Response.WriteJsonAsync(Entries);
    }
}