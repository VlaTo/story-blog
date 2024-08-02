namespace StoryBlog.Web.Identity.Client.Requests;

public class JsonWebKeySetRequest : ProtocolRequest
{
    public JsonWebKeySetRequest()
    {
    }

    public JsonWebKeySetRequest(ProtocolRequest source)
        : base(source)
    {
    }
}