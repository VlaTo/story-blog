using Microsoft.IdentityModel.Tokens;

namespace StoryBlog.Web.Identity.Client.Responses;

public class JsonWebKeySetResponse : ProtocolResponse
{
    public JsonWebKeySet? KeySet
    {
        get; 
        set;
    }

    protected override Task InitializeAsync(object? initializationData = null)
    {
        if (true != HttpResponse?.IsSuccessStatusCode)
        {
            ErrorMessage = initializationData as string;
        }
        else
        {
            KeySet = new JsonWebKeySet(Raw!);
        }

        return Task.CompletedTask;
    }
}