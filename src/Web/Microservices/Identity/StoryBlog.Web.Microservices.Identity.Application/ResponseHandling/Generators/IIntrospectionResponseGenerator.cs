using StoryBlog.Web.Microservices.Identity.Application.Validation.Results;

namespace StoryBlog.Web.Microservices.Identity.Application.ResponseHandling.Generators;

/// <summary>
/// Interface for introspection response generator
/// </summary>
public interface IIntrospectionResponseGenerator
{
    /// <summary>
    /// Processes the response.
    /// </summary>
    /// <param name="validationResult">The validation result.</param>
    /// <returns></returns>
    Task<Dictionary<string, object>> ProcessAsync(IntrospectionRequestValidationResult validationResult);
}