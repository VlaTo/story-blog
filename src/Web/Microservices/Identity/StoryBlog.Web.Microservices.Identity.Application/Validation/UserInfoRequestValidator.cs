using IdentityModel;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Microservices.Identity.Application.Contexts;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Application.Services;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Results;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation;

/// <summary>
/// Default userinfo request validator
/// </summary>
/// <seealso cref="IUserInfoRequestValidator" />
internal sealed class UserInfoRequestValidator : IUserInfoRequestValidator
{
    private readonly ITokenValidator tokenValidator;
    private readonly IProfileService profile;
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserInfoRequestValidator" /> class.
    /// </summary>
    /// <param name="tokenValidator">The token validator.</param>
    /// <param name="profile">The profile service</param>
    /// <param name="logger">The logger.</param>
    public UserInfoRequestValidator(
        ITokenValidator tokenValidator,
        IProfileService profile,
        ILogger<UserInfoRequestValidator> logger)
    {
        this.tokenValidator = tokenValidator;
        this.profile = profile;
        this.logger = logger;
    }

    /// <summary>
    /// Validates a userinfo request.
    /// </summary>
    /// <param name="accessToken">The access token.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public async Task<UserInfoRequestValidationResult> ValidateRequestAsync(string accessToken)
    {
        using var activity = Tracing.BasicActivitySource.StartActivity("UserInfoRequestValidator.ValidateRequest");

        // the access token needs to be valid and have at least the openid scope
        var tokenResult = await tokenValidator.ValidateAccessTokenAsync(
            accessToken,
            OidcConstants.StandardScopes.OpenId
        );

        if (tokenResult.IsError)
        {
            return new UserInfoRequestValidationResult
            {
                IsError = true,
                Error = tokenResult.Error
            };
        }

        // the token must have a one sub claim
        var subClaim = tokenResult.Claims.SingleOrDefault(c => c.Type == JwtClaimTypes.Subject);

        if (null == subClaim)
        {
            logger.LogError("Token contains no sub claim");

            return new UserInfoRequestValidationResult
            {
                IsError = true,
                Error = OidcConstants.ProtectedResourceErrors.InvalidToken
            };
        }

        // create subject from incoming access token
        var claims = tokenResult.Claims.Where(x => !Common.Identity.Permission.Constants.Filters.ProtocolClaimsFilter.Contains(x.Type));
        var subject = Principal.Create("UserInfo", claims.ToArray());

        // make sure user is still active
        var isActiveContext = new IsActiveContext(
            subject,
            tokenResult.Client,
            IdentityServerConstants.ProfileIsActiveCallers.UserInfoRequestValidation
        );

        await profile.IsActiveAsync(isActiveContext);

        if (false == isActiveContext.IsActive)
        {
            logger.LogError("User is not active: {sub}", subject.GetSubjectId());

            return new UserInfoRequestValidationResult
            {
                IsError = true,
                Error = OidcConstants.ProtectedResourceErrors.InvalidToken
            };
        }

        return new UserInfoRequestValidationResult
        {
            IsError = false,
            TokenValidationResult = tokenResult,
            Subject = subject
        };
    }
}