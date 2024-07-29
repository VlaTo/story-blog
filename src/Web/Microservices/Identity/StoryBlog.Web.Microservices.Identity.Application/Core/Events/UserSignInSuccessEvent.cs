using StoryBlog.Web.Common.Identity.Permission;

namespace StoryBlog.Web.Microservices.Identity.Application.Core.Events;

/// <summary>
/// Event for successful user authentication
/// </summary>
/// <seealso cref="Event" />
public class UserSignInSuccessEvent : Event
{
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    /// <value>
    /// The username.
    /// </value>
    public string Username
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the provider.
    /// </summary>
    /// <value>
    /// The provider.
    /// </value>
    public string Provider
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the provider user identifier.
    /// </summary>
    /// <value>
    /// The provider user identifier.
    /// </value>
    public string ProviderUserId
    {
        get;
        set;
    }

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
    /// Gets or sets the display name.
    /// </summary>
    /// <value>
    /// The display name.
    /// </value>
    public string? DisplayName
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the endpoint.
    /// </summary>
    /// <value>
    /// The endpoint.
    /// </value>
    public string Endpoint
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the client id.
    /// </summary>
    /// <value>
    /// The client id.
    /// </value>
    public string? ClientId
    {
        get;
        set;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserSignInSuccessEvent"/> class.
    /// </summary>
    /// <param name="provider">The provider.</param>
    /// <param name="providerUserId">The provider user identifier.</param>
    /// <param name="subjectId">The subject identifier.</param>
    /// <param name="name">The name.</param>
    /// <param name="interactive">if set to <c>true</c> [interactive].</param>
    /// <param name="clientId">The client id.</param>
    public UserSignInSuccessEvent(
        string provider,
        string providerUserId,
        string subjectId,
        string? name,
        bool interactive = true,
        string? clientId = null)
        : this()
    {
        Provider = provider;
        ProviderUserId = providerUserId;
        SubjectId = subjectId;
        DisplayName = name;

        if (interactive)
        {
            Endpoint = "UI";
        }
        else
        {
            Endpoint = Constants.EndpointNames.Token;
        }

        ClientId = clientId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserSignInSuccessEvent"/> class.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="subjectId">The subject identifier.</param>
    /// <param name="name">The name.</param>
    /// <param name="interactive">if set to <c>true</c> [interactive].</param>
    /// <param name="clientId">The client id.</param>
    public UserSignInSuccessEvent(
        string username,
        string subjectId,
        string? name,
        bool interactive = true,
        string? clientId = null)
        : this()
    {
        Username = username;
        SubjectId = subjectId;
        DisplayName = name;
        ClientId = clientId;

        if (interactive)
        {
            Endpoint = "UI";
        }
        else
        {
            Endpoint = Constants.EndpointNames.Token;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserSignInSuccessEvent"/> class.
    /// </summary>
    protected UserSignInSuccessEvent()
        : base(EventCategories.Authentication,
            "User Login Success",
            EventTypes.Success,
            EventIds.UserLoginSuccess)
    {
    }
}