namespace StoryBlog.Web.Microservices.Identity.Application.Storage;

/// <summary>
/// Represents the permissions (in terms of scopes) granted to a client by a subject
/// </summary>
public class Consent
{
    /// <summary>
    /// Gets or sets the subject identifier.
    /// </summary>
    /// <value>
    /// The subject identifier.
    /// </value>
    public string SubjectId
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the client identifier.
    /// </summary>
    /// <value>
    /// The client identifier.
    /// </value>
    public string ClientId
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the scopes.
    /// </summary>
    /// <value>
    /// The scopes.
    /// </value>
    public IReadOnlyCollection<string>? Scopes
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the creation time.
    /// </summary>
    /// <value>
    /// The creation time.
    /// </value>
    public DateTimeOffset CreationTime
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the expiration.
    /// </summary>
    /// <value>
    /// The expiration.
    /// </value>
    public DateTimeOffset? Expiration
    {
        get;
        set;
    }
}