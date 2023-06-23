namespace StoryBlog.Web.Microservices.Identity.Application.Core.Events;

/// <summary>
/// Event for successful API authentication
/// </summary>
/// <seealso cref="Event" />
public sealed class ApiAuthenticationSuccessEvent : Event
{
    /// <summary>
    /// Gets or sets the name of the API.
    /// </summary>
    /// <value>
    /// The name of the API.
    /// </value>
    public string ApiName
    {
        get; 
        set;
    }

    /// <summary>
    /// Gets or sets the authentication method.
    /// </summary>
    /// <value>
    /// The authentication method.
    /// </value>
    public string AuthenticationMethod
    {
        get; 
        set;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiAuthenticationSuccessEvent"/> class.
    /// </summary>
    /// <param name="apiName">Name of the API.</param>
    /// <param name="authenticationMethod">The authentication method.</param>
    public ApiAuthenticationSuccessEvent(string apiName, string authenticationMethod)
        : base(EventCategories.Authentication,
            "API Authentication Success",
            EventTypes.Success,
            EventIds.ApiAuthenticationSuccess)
    {
        ApiName = apiName;
        AuthenticationMethod = authenticationMethod;
    }
}