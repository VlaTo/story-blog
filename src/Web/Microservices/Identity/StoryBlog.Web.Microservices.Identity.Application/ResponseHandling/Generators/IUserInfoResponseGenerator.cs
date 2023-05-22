using StoryBlog.Web.Microservices.Identity.Application.Validation.Results;

namespace StoryBlog.Web.Microservices.Identity.Application.ResponseHandling.Generators;

/// <summary>
/// Interface for the userinfo response generator
/// </summary>
public interface IUserInfoResponseGenerator
{
    /// <summary>
    /// Creates the response.
    /// </summary>
    /// <param name="validationResult">The userinfo request validation result.</param>
    /// <returns></returns>
    Task<Dictionary<string, object>> ProcessAsync(UserInfoRequestValidationResult validationResult);
}