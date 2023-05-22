using StoryBlog.Web.Microservices.Identity.Application.Validation.Requests;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Results;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation.Contexts;

/// <summary>
/// Class describing the resource owner password validation context
/// </summary>
public class ResourceOwnerPasswordValidationContext
{
    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    /// <value>
    /// The name of the user.
    /// </value>
    public string? UserName
    {
        get;
        init;
    }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    /// <value>
    /// The password.
    /// </value>
    public string? Password
    {
        get;
        init;
    }

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

    public ResourceOwnerPasswordValidationContext(string? userName, string? password, ValidatedTokenRequest request)
    {
        UserName = userName;
        Password = password;
        Request = request;
        Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
    }

    public void SetResult(GrantValidationResult result)
    {
        Result = result;
    }
}