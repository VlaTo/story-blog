using StoryBlog.Web.Identity;
using StoryBlog.Web.Microservices.Identity.Application.Storage;
using JwtClaimTypes = StoryBlog.Web.Common.Identity.Permission.JwtClaimTypes;

namespace StoryBlog.Web.Microservices.Identity.Application.Models;

/// <summary>
/// Convenience class that defines standard identity resources.
/// </summary>
public static class IdentityResources
{
    #region OpenId

    /// <summary>
    /// Models the standard openid scope
    /// </summary>
    /// <seealso cref="IdentityResource" />
    public class OpenId : IdentityResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenId"/> class.
        /// </summary>
        public OpenId()
            : base(OidcConstants.StandardScopes.OpenId)
        {
            //Name = IdentityServerConstants.StandardScopes.OpenId;
            DisplayName = "Your user identifier";
            Required = true;
            UserClaims.Add(JwtClaimTypes.Subject);
        }
    }

    #endregion

    #region Profile

    /// <summary>
    /// Models the standard profile scope
    /// </summary>
    /// <seealso cref="IdentityResource" />
    public class Profile : IdentityResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Profile"/> class.
        /// </summary>
        public Profile()
            : base(OidcConstants.StandardScopes.Profile)
        {
            //Name = IdentityServerConstants.StandardScopes.Profile;
            DisplayName = "User profile";
            Description = "Your user profile information (first name, last name, etc.)";
            Emphasize = true;
            UserClaims = IdentityServerConstants.ScopeToClaimsMapping[OidcConstants.StandardScopes.Profile].ToArray();
        }
    }

    #endregion
}