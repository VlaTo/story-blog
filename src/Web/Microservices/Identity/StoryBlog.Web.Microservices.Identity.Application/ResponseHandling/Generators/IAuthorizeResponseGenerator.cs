using StoryBlog.Web.Microservices.Identity.Application.Authorization.Responses;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Requests;

namespace StoryBlog.Web.Microservices.Identity.Application.ResponseHandling.Generators;

/// <summary>
/// Interface for the authorize response generator
/// </summary>
public interface IAuthorizeResponseGenerator
{
    /// <summary>
    /// Creates the response
    /// </summary>
    /// <param name="request">The request.</param>
    /// <returns></returns>
    Task<AuthorizeResponse> CreateResponseAsync(ValidatedAuthorizeRequest request);
}