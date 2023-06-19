using StoryBlog.Web.Microservices.Identity.Application.Validation.Results;
using System.Collections.Specialized;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation;

/// <summary>
/// Interface for the backchannel authentication request validator
/// </summary>
public interface IBackchannelAuthenticationRequestValidator
{
    /// <summary>
    /// Validates the request.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    /// <param name="clientValidationResult">The client validation result.</param>
    /// <returns></returns>
    Task<BackchannelAuthenticationRequestValidationResult> ValidateRequestAsync(NameValueCollection parameters, ClientSecretValidationResult clientValidationResult);
}