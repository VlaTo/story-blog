using System.Security.Claims;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation.Results;

/// <summary>
/// Models the result of JWT request validation.
/// </summary>
public class JwtRequestValidationResult : ValidationResult
{
    /// <summary>
    /// The key/value pairs from the JWT payload of a successfuly validated request.
    /// </summary>
    public IEnumerable<Claim> Payload
    {
        get;
        set;
    }
}