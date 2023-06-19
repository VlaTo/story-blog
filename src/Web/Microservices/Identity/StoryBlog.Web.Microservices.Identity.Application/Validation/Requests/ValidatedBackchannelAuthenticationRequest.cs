using System.Security.Claims;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation.Requests;

/// <summary>
/// Models a validated request to the backchannel authentication endpoint.
/// </summary>
public sealed class ValidatedBackchannelAuthenticationRequest : ValidatedRequest
{
    /// <summary>
    /// Gets or sets the scopes.
    /// </summary>
    public ICollection<string>? RequestedScopes
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the resource indicator.
    /// </summary>
    public ICollection<string>? RequestedResourceIndicators
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the authentication context reference classes.
    /// </summary>
    public ICollection<string>? AuthenticationContextReferenceClasses
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the tenant.
    /// </summary>
    public string? Tenant
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the idp.
    /// </summary>
    public string? IdP
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the login hint token.
    /// </summary>
    public string? LoginHintToken
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the id token hint.
    /// </summary>
    public string? IdTokenHint
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the validated claims from the id token hint.
    /// </summary>
    public IEnumerable<Claim>? IdTokenHintClaims
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the login hint.
    /// </summary>
    public string? LoginHint
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the binding message.
    /// </summary>
    public string? BindingMessage
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the user code.
    /// </summary>
    public string? UserCode
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the requested expiry if present, otherwise the client configured expiry.
    /// </summary>
    public TimeSpan Expiry
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the validated contents of the request object (if present)
    /// </summary>
    public IEnumerable<Claim> RequestObjectValues
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the request object (either passed by value or retrieved by reference)
    /// </summary>
    public string? RequestObject
    {
        get;
        set;
    }

    public ValidatedBackchannelAuthenticationRequest()
    {
        RequestObjectValues = new List<Claim>();
    }
}