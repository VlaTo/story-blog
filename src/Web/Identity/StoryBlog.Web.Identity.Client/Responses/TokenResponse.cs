namespace StoryBlog.Web.Identity.Client.Responses;

public sealed class TokenResponse : ProtocolResponse
{
    public string? AccessToken => GetStringOrDefault(OidcConstants.TokenResponse.AccessToken);

    public string? IdentityToken => GetStringOrDefault(OidcConstants.TokenResponse.IdentityToken);

    public string? Scope => GetStringOrDefault(OidcConstants.TokenResponse.Scope);

    public string? IssuedTokenType => GetStringOrDefault(OidcConstants.TokenResponse.IssuedTokenType);

    public string? TokenType => GetStringOrDefault(OidcConstants.TokenResponse.TokenType);

    public string? RefreshToken => GetStringOrDefault(OidcConstants.TokenResponse.RefreshToken);

    public string? ErrorDescription => GetStringOrDefault(OidcConstants.TokenResponse.ErrorDescription);

    public int ExpiresIn
    {
        get
        {
            var value = GetStringOrDefault(OidcConstants.TokenResponse.ExpiresIn);

            if (null != value)
            {
                if (Int32.TryParse(value, out var theValue))
                {
                    return theValue;
                }
            }

            return 0;
        }
    }
}