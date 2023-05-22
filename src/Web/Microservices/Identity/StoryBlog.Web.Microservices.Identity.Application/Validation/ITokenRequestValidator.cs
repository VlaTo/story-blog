using StoryBlog.Web.Microservices.Identity.Application.Validation.Results;
using System.Collections.Specialized;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation;

/// <summary>
/// Interface for the token request validator
/// </summary>
public interface ITokenRequestValidator
{
    /// <summary>
    /// Validates the request.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    /// <param name="clientValidationResult">The client validation result.</param>
    /// <returns></returns>
    Task<TokenRequestValidationResult> ValidateRequestAsync(
        NameValueCollection parameters,
        ClientSecretValidationResult clientValidationResult
    );
}