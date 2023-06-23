using System.Collections.Specialized;
using StoryBlog.Web.Microservices.Identity.Application.Storage;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Results;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation;

/// <summary>
/// Interface for the introspection request validator
/// </summary>
public interface IIntrospectionRequestValidator
{
    /// <summary>
    /// Validates the request.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    /// <param name="api">The API.</param>
    /// <returns></returns>
    Task<IntrospectionRequestValidationResult> ValidateAsync(NameValueCollection parameters, ApiResource api);
}