namespace StoryBlog.Web.Microservices.Identity.Application.Validation.Results;

/// <summary>
/// Failure reasons for introspection request
/// </summary>
public enum IntrospectionRequestValidationFailureReason
{
    /// <summary>
    /// none
    /// </summary>
    None,

    /// <summary>
    /// missing token
    /// </summary>
    MissingToken,

    /// <summary>
    /// invalid token
    /// </summary>
    InvalidToken,

    /// <summary>
    /// invalid scope
    /// </summary>
    InvalidScope
}