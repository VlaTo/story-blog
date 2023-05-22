using IdentityModel;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Application.Models;
using StoryBlog.Web.Microservices.Identity.Application.ResponseHandling.Generators;
using StoryBlog.Web.Microservices.Identity.Application.Services;
using StoryBlog.Web.Microservices.Identity.Application.Storage;
using StoryBlog.Web.Microservices.Identity.Application.Stores;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Contexts;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Results;
using System.Security.Claims;

namespace StoryBlog.Web.Microservices.Identity.Application.ResponseHandling.Defaults;

/// <summary>
/// The userinfo response generator
/// </summary>
/// <seealso cref="IUserInfoResponseGenerator" />
public class DefaultUserInfoResponseGenerator : IUserInfoResponseGenerator
{
    /// <summary>
    /// The logger
    /// </summary>
    protected ILogger Logger
    {
        get;
    }

    /// <summary>
    /// The profile service
    /// </summary>
    protected IProfileService Profile
    {
        get;
    }

    /// <summary>
    /// The resource store
    /// </summary>
    protected IResourceStore Resources
    {
        get;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultUserInfoResponseGenerator"/> class.
    /// </summary>
    /// <param name="profile">The profile.</param>
    /// <param name="resourceStore">The resource store.</param>
    /// <param name="logger">The logger.</param>
    public DefaultUserInfoResponseGenerator(
        IProfileService profile,
        IResourceStore resourceStore,
        ILogger<DefaultUserInfoResponseGenerator> logger)
    {
        Profile = profile;
        Resources = resourceStore;
        Logger = logger;
    }

    /// <summary>
    /// Creates the response.
    /// </summary>
    /// <param name="validationResult">The userinfo request validation result.</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">Profile service returned incorrect subject value</exception>
    public virtual async Task<Dictionary<string, object>> ProcessAsync(UserInfoRequestValidationResult validationResult)
    {
        using var activity = Tracing.BasicActivitySource.StartActivity("UserInfoResponseGenerator.Process");

        Logger.LogDebug("Creating userinfo response");

        // extract scopes and turn into requested claim types
        var scopes = validationResult.TokenValidationResult.Claims
            .Where(c => c.Type == JwtClaimTypes.Scope)
            .Select(c => c.Value)
            .ToArray();

        var validatedResources = await GetRequestedResourcesAsync(scopes);
        var requestedClaimTypes = await GetRequestedClaimTypesAsync(validatedResources);

        Logger.LogDebug("Requested claim types: {claimTypes}", requestedClaimTypes.ToSpaceSeparatedString());

        // call profile service
        var context = new ProfileDataRequestContext(
            validationResult.Subject,
            validationResult.TokenValidationResult.Client,
            IdentityServerConstants.ProfileDataCallers.UserInfoEndpoint,
            requestedClaimTypes
        )
        {
            RequestedResources = validatedResources
        };

        await Profile.GetProfileDataAsync(context);

        var profileClaims = context.IssuedClaims;

        // construct outgoing claims
        var outgoingClaims = new List<Claim>();

        if (0 == profileClaims.Count)
        {
            Logger.LogInformation("Profile service returned no claims (null)");
        }
        else
        {
            outgoingClaims.AddRange(profileClaims);
            Logger.LogInformation("Profile service returned the following claim types: {types}", profileClaims.Select(c => c.Type).ToSpaceSeparatedString());
        }

        var subClaim = outgoingClaims.SingleOrDefault(x => x.Type == JwtClaimTypes.Subject);

        if (null == subClaim)
        {
            outgoingClaims.Add(new Claim(JwtClaimTypes.Subject, validationResult.Subject.GetSubjectId()));
        }
        else if (subClaim.Value != validationResult.Subject.GetSubjectId())
        {
            Logger.LogError("Profile service returned incorrect subject value: {sub}", subClaim);
            throw new InvalidOperationException("Profile service returned incorrect subject value");
        }

        return outgoingClaims.ToClaimsDictionary();
    }

    /// <summary>
    ///  Gets the identity resources from the scopes.
    /// </summary>
    /// <param name="scopes"></param>
    /// <returns></returns>
    protected internal virtual async Task<ResourceValidationResult?> GetRequestedResourcesAsync(IReadOnlyCollection<string>? scopes)
    {
        if (null == scopes || 0 == scopes.Count)
        {
            return null;
        }

        var scopeString = string.Join(' ', scopes);
        Logger.LogDebug("Scopes in access token: {scopes}", scopeString);

        // if we ever parameterized identity scopes, then we would need to invoke the resource validator's parse API here
        var identityResources = await Resources.FindEnabledIdentityResourcesByScopeAsync(scopes);

        var resources = Storage.Resources.Create(identityResources, Enumerable.Empty<ApiResource>(), Enumerable.Empty<ApiScope>());
        var result = new ResourceValidationResult(resources);

        return result;
    }

    /// <summary>
    /// Gets the requested claim types.
    /// </summary>
    /// <param name="resourceValidationResult"></param>
    /// <returns></returns>
    protected internal virtual Task<IEnumerable<string>> GetRequestedClaimTypesAsync(ResourceValidationResult? resourceValidationResult)
    {
        if (null == resourceValidationResult)
        {
            return Task.FromResult(Enumerable.Empty<string>());
        }

        var identityResources = resourceValidationResult.Resources.IdentityResources;
        var result = identityResources.SelectMany(x => x.UserClaims).Distinct();

        return Task.FromResult(result);
    }
}