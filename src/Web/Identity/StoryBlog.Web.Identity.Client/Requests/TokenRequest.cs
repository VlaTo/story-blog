namespace StoryBlog.Web.Identity.Client.Requests;

public sealed class TokenRequest : ProtocolRequest
{
    public required string GrantType
    {
        get; 
        set;
    }
}