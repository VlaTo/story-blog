﻿using StoryBlog.Web.Common.Identity.Permission;
using StoryBlog.Web.Identity;
using StoryBlog.Web.Microservices.Identity.Application.Configuration;
using StoryBlog.Web.Microservices.Identity.Application.Storage;

namespace StoryBlog.Web.Microservices.Identity.Application.DependencyInjection.Options;

/// <summary>
/// A builder for Clients.
/// </summary>
public class ClientBuilder
{
    private const string NativeAppClientRedirectUri = "urn:ietf:wg:oauth:2.0:oob";

    readonly Client client;
    private bool built;

    /// <summary>
    /// Creates a new builder for a single page application that coexists with an authorization server.
    /// </summary>
    /// <param name="clientId">The client id for the single page application.</param>
    /// <returns>A <see cref="ClientBuilder"/>.</returns>
    public static ClientBuilder IdentityServerSPA(string clientId)
    {
        var client = CreateClient(clientId);

        return new ClientBuilder(client)
            .WithApplicationProfile(ApplicationProfiles.IdentityServerSPA)
            .WithAllowedGrants(Models.GrantTypes.Code)
            .WithoutClientSecrets()
            .WithPkce()
            .WithAllowedOrigins(Array.Empty<string>())
            .AllowAccessTokensViaBrowser();
    }

    /// <summary>
    /// Creates a new builder for an externally registered single page application.
    /// </summary>
    /// <param name="clientId">The client id for the single page application.</param>
    /// <returns>A <see cref="ClientBuilder"/>.</returns>
    public static ClientBuilder SPA(string clientId)
    {
        var client = CreateClient(clientId);

        return new ClientBuilder(client)
            .WithApplicationProfile(ApplicationProfiles.SPA)
            .WithAllowedGrants(Models.GrantTypes.Code)
            .WithoutClientSecrets()
            .WithPkce()
            .AllowAccessTokensViaBrowser();
    }

    /// <summary>
    /// Creates a new builder for an externally registered native application.
    /// </summary>
    /// <param name="clientId">The client id for the native application.</param>
    /// <returns>A <see cref="ClientBuilder"/>.</returns>
    public static ClientBuilder NativeApp(string clientId)
    {
        var client = CreateClient(clientId);

        return new ClientBuilder(client)
            .WithApplicationProfile(ApplicationProfiles.NativeApp)
            .WithAllowedGrants(Models.GrantTypes.Code)
            .WithRedirectUri(NativeAppClientRedirectUri)
            .WithLogoutRedirectUri(NativeAppClientRedirectUri)
            .WithPkce()
            .WithoutClientSecrets()
            .WithScopes(OidcConstants.StandardScopes.OfflineAccess);
    }

    /// <summary>
    /// Initializes a new instance of <see cref="ClientBuilder"/>.
    /// </summary>
    public ClientBuilder()
        : this(new Client())
    {
    }

    /// <summary>
    /// Initializes a new intance of <see cref="ClientBuilder"/>.
    /// </summary>
    /// <param name="client">A preconfigured client.</param>
    public ClientBuilder(Client client)
    {
        this.client = client;
    }

    /// <summary>
    /// Updates the client id (and name) of the client.
    /// </summary>
    /// <param name="clientId">The new client id.</param>
    /// <returns>The <see cref="ClientBuilder"/>.</returns>
    public ClientBuilder WithClientId(string clientId)
    {
        client.ClientId = clientId;
        client.ClientName = clientId;

        return this;
    }

    /// <summary>
    /// Sets the application profile for the client.
    /// </summary>
    /// <param name="profile">The the profile for the application from <see cref="ApplicationProfiles"/>.</param>
    /// <returns>The <see cref="ClientBuilder"/>.</returns>
    public ClientBuilder WithApplicationProfile(string profile)
    {
        client.Properties.Add(ApplicationProfilesPropertyNames.Profile, profile);
        return this;
    }

    /// <summary>
    /// Adds the <paramref name="scopes"/> to the list of allowed scopes for the client.
    /// </summary>
    /// <param name="scopes">The list of scopes.</param>
    /// <returns>The <see cref="ClientBuilder"/>.</returns>
    public ClientBuilder WithScopes(params string[] scopes)
    {
        foreach (var scope in scopes)
        {
            client.AllowedScopes.Add(scope);
        }

        return this;
    }

    /// <summary>
    /// Adds the <paramref name="redirectUri"/> to the list of valid redirect uris for the client.
    /// </summary>
    /// <param name="redirectUri">The redirect uri to add.</param>
    /// <returns>The <see cref="ClientBuilder"/>.</returns>
    public ClientBuilder WithRedirectUri(string redirectUri)
    {
        client.RedirectUris.Add(redirectUri);
        return this;
    }

    /// <summary>
    /// Adds the <paramref name="logoutUri"/> to the list of valid logout redirect uris for the client.
    /// </summary>
    /// <param name="logoutUri">The logout uri to add.</param>
    /// <returns>The <see cref="ClientBuilder"/>.</returns>
    public ClientBuilder WithLogoutRedirectUri(string logoutUri)
    {
        client.PostLogoutRedirectUris.Add(logoutUri);
        return this;
    }

    /// <summary>
    /// Adds the <paramref name="clientSecret"/> to the list of client secrets for the client and configures the client to
    /// require using the secret when getting tokens from the token endpoint.
    /// </summary>
    /// <param name="clientSecret">The client secret to add.</param>
    /// <returns>The <see cref="ClientBuilder"/>.</returns>
    public ClientBuilder WithClientSecret(string clientSecret)
    {
        client.ClientSecrets.Add(new Secret(clientSecret));
        client.RequireClientSecret = true;

        return this;
    }

    /// <summary>
    /// Removes any configured client secret from the client and configures it to not require a client secret for getting tokens
    /// from the token endpoint.
    /// </summary>
    /// <returns>The <see cref="ClientBuilder"/>.</returns>
    public ClientBuilder WithoutClientSecrets()
    {
        client.RequireClientSecret = false;
        client.ClientSecrets.Clear();

        return this;
    }

    /// <summary>
    /// Builds the client.
    /// </summary>
    /// <returns>The built <see cref="Client"/>.</returns>
    public Client Build()
    {
        if (built)
        {
            throw new InvalidOperationException("Client already built.");
        }

        built = true;

        return client;
    }

    internal ClientBuilder WithPkce()
    {
        client.RequirePkce = true;
        client.AllowPlainTextPkce = false;

        return this;
    }

    internal ClientBuilder FromConfiguration()
    {
        client.Properties[ApplicationProfilesPropertyNames.Source] = ApplicationProfilesPropertyValues.Configuration;
        return this;
    }

    internal ClientBuilder WithAllowedGrants(ICollection<string> grants)
    {
        client.AllowedGrantTypes = grants;
        return this;
    }

    internal ClientBuilder WithAllowedOrigins(params string[] origins)
    {
        client.AllowedCorsOrigins = origins;
        return this;
    }

    internal ClientBuilder AllowAccessTokensViaBrowser()
    {
        client.AllowAccessTokensViaBrowser = true;
        return this;
    }

    private static Client CreateClient(string name)
    {
        var client = new Client
        {
            ClientId = name,
            ClientName = name,
            RequireConsent = false
        };

        return client;
    }
}