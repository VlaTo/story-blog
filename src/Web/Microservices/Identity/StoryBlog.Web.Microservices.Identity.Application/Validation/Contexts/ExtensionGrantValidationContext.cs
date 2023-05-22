using StoryBlog.Web.Microservices.Identity.Application.Validation.Requests;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Results;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation.Contexts;

/// <summary>
/// Class describing the extension grant validation context
/// </summary>
public class ExtensionGrantValidationContext
{
    /// <summary>
    /// Gets or sets the request.
    /// </summary>
    /// <value>
    /// The request.
    /// </value>
    public ValidatedTokenRequest Request
    {
        get;
        init;
    }

    /// <summary>
    /// Gets or sets the result.
    /// </summary>
    /// <value>
    /// The result.
    /// </value>
    public GrantValidationResult Result
    {
        get;
        private set;
    }

    public ExtensionGrantValidationContext(ValidatedTokenRequest request)
    {
        Request = request;
        Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
    }

    public void SetResult(GrantValidationResult result)
    {
        Result = result;
    }
}