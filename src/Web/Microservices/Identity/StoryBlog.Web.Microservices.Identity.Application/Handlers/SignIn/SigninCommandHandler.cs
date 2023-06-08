using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StoryBlog.Infrastructure.AnyOf;
using StoryBlog.Web.Microservices.Identity.Application.Configuration;
using StoryBlog.Web.Microservices.Identity.Application.Core.Events;
using StoryBlog.Web.Microservices.Identity.Application.Services;
using StoryBlog.Web.Microservices.Identity.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;

namespace StoryBlog.Web.Microservices.Identity.Application.Handlers.SigningIn;

public sealed class SigninCommandHandler : IRequestHandler<SigninCommand, AnyOf<SignInError, (StoryBlogUser User, string? Token)>>
{
    private readonly UserManager<StoryBlogUser> userManager;
    private readonly SignInManager<StoryBlogUser> signInManager;
    private readonly RoleManager<StoryBlogUserRole> roleManager;
    private readonly ISystemClock clock;
    private readonly IAuthenticationEventSink eventSink;
    private readonly StoryBlogIdentityServerOptions<StoryBlogIdentityOptions> options;
    private readonly ILogger<SigninCommandHandler> logger;

    public SigninCommandHandler(
        UserManager<StoryBlogUser> userManager,
        SignInManager<StoryBlogUser> signInManager,
        RoleManager<StoryBlogUserRole> roleManager,
        ISystemClock clock,
        IAuthenticationEventSink eventSink,
        IOptions<StoryBlogIdentityServerOptions<StoryBlogIdentityOptions>> options,
        ILogger<SigninCommandHandler> logger)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.roleManager = roleManager;
        this.clock = clock;
        this.eventSink = eventSink;
        this.options = options.Value;
        this.logger = logger;
    }

    public async Task<AnyOf<SignInError, (StoryBlogUser User, string? Token)>> Handle(SigninCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (null == user)
        {
            return SignInError.NotFound;
        }

        if (false == user.EmailConfirmed)
        {
            return SignInError.NotAllowed;
        }

        var canSignIn = await signInManager.CanSignInAsync(user);

        if (canSignIn)
        {
            var signinResult = await signInManager.PasswordSignInAsync(
                user,
                request.Password,
                options.AllowRememberMe && request.RememberMe,
                options.AllowUserLockOut
            );

            if (signinResult.Succeeded)
            {
                var successEvent = new UserSignInSuccessEvent(
                    user.UserName,
                    user.Id,
                    user.UserName,
                    clientId: request.Context?.Client?.ClientId
                );

                await eventSink.RaiseEventAsync(successEvent);

                if (null != request.Context)
                {

                    if (request.Context.IsNativeClient())
                    {
                        user.RefreshToken = GenerateRefreshToken();
                        user.RefreshTokenExpiryTime = clock.UtcNow.Add(options.RefreshTokenDuration).DateTime;

                        var token = await GenerateJwtAsync(user);

                        return (user, token);
                    }
                }

                return (user, null);
            }

            if (signinResult.IsLockedOut)
            {
                return SignInError.LockedOut;
            }

            if (signinResult.RequiresTwoFactor)
            {
                return SignInError.Unauthorized;
            }
        }

        return SignInError.Unknown;
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var generator = RandomNumberGenerator.Create();

        generator.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    private async Task<string> GenerateJwtAsync(StoryBlogUser user)
    {
        var credentials = GetSigningCredentials();
        var claimsAsync = await GetClaimsAsync(user);

        return GenerateEncryptedToken(credentials, claimsAsync, options.SecurityTokenDuration);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var secret = Encoding.UTF8.GetBytes(options.Secret);
        var securityKey = new SymmetricSecurityKey(secret);
        return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    }

    private static string GenerateEncryptedToken(
        SigningCredentials signingCredentials,
        IEnumerable<Claim> claims,
        TimeSpan duration)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.WriteToken(new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.Add(duration),
            signingCredentials: signingCredentials
        ));
    }

    private async Task<IReadOnlyCollection<Claim>> GetClaimsAsync(StoryBlogUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName!)
        };

        if (userManager.SupportsUserEmail)
        {
            claims.Add(new(ClaimTypes.Email, user.Email!));
        }

        if (userManager.SupportsUserPhoneNumber && false == String.IsNullOrEmpty(user.PhoneNumber))
        {
            claims.Add(new(ClaimTypes.MobilePhone, user.PhoneNumber));
        }

        if (userManager.SupportsUserRole)
        {
            var permissions = new List<Claim>();
            var roleNames = await userManager.GetRolesAsync(user);

            foreach (var roleName in roleNames)
            {
                var userRole = await roleManager.FindByNameAsync(roleName);

                if (null == userRole)
                {
                    continue;
                }

                var userRoleClaims = await roleManager.GetClaimsAsync(userRole);

                claims.Add(new Claim(ClaimTypes.Role, roleName));
                permissions.AddRange(userRoleClaims);
            }

            claims.AddRange(permissions);
        }

        if (userManager.SupportsUserClaim)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);
        }

        return claims;
    }
}