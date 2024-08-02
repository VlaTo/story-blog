namespace StoryBlog.Web.Identity.Client.Requests;

public class ClientCredentialsTokenRequest : ProtocolRequest
{
    public ClientCredentialsTokenRequest()
    {
    }

    internal ClientCredentialsTokenRequest(ProtocolRequest source)
        : base(source)
    {
    }

    public string? Scope
    {
        get; 
        set;
    }

    public ICollection<string> Resource
    {
        get; 
        set;
    } = new HashSet<string>();
}