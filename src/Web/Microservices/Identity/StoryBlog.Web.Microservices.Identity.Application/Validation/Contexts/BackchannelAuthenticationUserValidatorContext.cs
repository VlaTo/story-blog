using System.Security.Claims;
using StoryBlog.Web.Microservices.Identity.Application.Storage;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation.Contexts;

/// <summary>
/// Context information for validating a user during backchannel authentication request.
/// </summary>
public sealed class BackchannelAuthenticationUserValidatorContext
{
    /// <summary>
    /// Gets or sets the client.
    /// </summary>
    public Client Client
    {
        get;
        init;
    }

    /// <summary>
    /// Gets or sets the login hint token.
    /// </summary>
    public string? LoginHintToken
    {
        get; 
        init;
    }

    /// <summary>
    /// Gets or sets the id token hint.
    /// </summary>
    public string? IdTokenHint
    {
        get;
        init;
    }

    /// <summary>
    /// Gets or sets the validated claims from the id token hint.
    /// </summary>
    public IEnumerable<Claim>? IdTokenHintClaims
    {
        get;
        init;
    }

    /// <summary>
    /// Gets or sets the login hint.
    /// </summary>
    public string? LoginHint
    {
        get;
        init;
    }

    /// <summary>
    /// Gets or sets the user code.
    /// </summary>
    public string? UserCode
    {
        get; 
        init;
    }

    /// <summary>
    /// Gets or sets the binding message.
    /// </summary>
    public string? BindingMessage
    {
        get; 
        init;
    }
}