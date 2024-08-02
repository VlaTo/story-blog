using System.Net.Http.Headers;
using System.Text;

namespace StoryBlog.Web.Identity.Client;

public class BasicAuthenticationOAuthHeaderValue : AuthenticationHeaderValue
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BasicAuthenticationOAuthHeaderValue"/> class.
    /// </summary>
    /// <param name="userName">Name of the user.</param>
    /// <param name="password">The password.</param>
    public BasicAuthenticationOAuthHeaderValue(string userName, string? password)
        : base(OidcConstants.AuthenticationSchemes.Basic, EncodeCredential(userName, password))
    {
    }

    /// <summary>
    /// Encodes the credential.
    /// </summary>
    /// <param name="userName">Name of the user.</param>
    /// <param name="password">The password.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">userName</exception>
    public static string EncodeCredential(string userName, string? password)
    {
        if (String.IsNullOrWhiteSpace(userName))
        {
            throw new ArgumentNullException(nameof(userName));
        }

        var credential = $"{UrlEncode(userName)}:{UrlEncode(password ?? String.Empty)}";

        return Convert.ToBase64String(Encoding.UTF8.GetBytes(credential));
    }

    private static string UrlEncode(string value)
    {
        return String.IsNullOrEmpty(value)
            ? String.Empty
            : Uri.EscapeDataString(value).Replace("%20", "+");
    }
}