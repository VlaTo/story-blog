using System.Net.Http.Headers;
using System.Text;

namespace StoryBlog.Web.Identity.Client;

public class BasicAuthenticationHeaderValue : AuthenticationHeaderValue
{
    public BasicAuthenticationHeaderValue(string userName, string? password)
        : base(OidcConstants.AuthenticationSchemes.Basic, EncodeCredential(userName, password))
    {
    }

    public static string EncodeCredential(string userName, string? password)
    {
        if (String.IsNullOrWhiteSpace(userName))
        {
            throw new ArgumentNullException(nameof(userName));
        }

        var credential = $"{userName}:{password ?? String.Empty}";

        return Convert.ToBase64String(Encoding.UTF8.GetBytes(credential));
    }
}