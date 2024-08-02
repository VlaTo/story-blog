using System.Net.Http.Headers;
using System.Net.Mime;

namespace StoryBlog.Web.Identity.Client.Requests;

public class ProtocolRequest : HttpRequestMessage
{
    public string? Address
    {
        get; 
        set;
    }

    public ClientAssertion ClientAssertion
    {
        get; 
        set;
    } = new();

    public string? ClientId
    {
        get; 
        set;
    }

    public string? ClientSecret
    {
        get;
        set;
    }

    public ClientCredentialType ClientCredentialType
    {
        get;
        set;
    } = ClientCredentialType.AuthorizationHeader;

    public BasicAuthenticationHeaderType AuthenticationHeaderType
    {
        get;
        set;
    } = BasicAuthenticationHeaderType.Rfc6749;

    public Parameters Parameters
    {
        get; 
        set;
    } = new();

    public ProtocolRequest()
    {
        Headers.Accept.Clear();
        Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
    }

    protected ProtocolRequest(ProtocolRequest source)
    {
        ApplyInternal(source);
    }

    public ProtocolRequest Clone() => Clone<ProtocolRequest>();

    public TRequest Clone<TRequest>() where TRequest : ProtocolRequest, new ()
    {
        var clone = new TRequest
        {
            RequestUri = RequestUri,
            Version = Version,
            Method = Method,

            Address = Address,
            AuthenticationHeaderType = AuthenticationHeaderType,
            ClientAssertion = ClientAssertion,
            ClientCredentialType = ClientCredentialType,
            ClientId = ClientId,
            ClientSecret = ClientSecret,
            //DPoPProofToken = DPoPProofToken,
            Parameters = new Parameters()
        };

        foreach (var item in Parameters)
        {
            clone.Parameters.Add(item);
        }

        clone.Headers.Clear();
        
        foreach (var header in Headers)
        {
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        foreach (var property in Options)
        {
            clone.Options.TryAdd(property.Key, property.Value);
        }

        return clone;
    }

    public void Prepare()
    {
        if (false == String.IsNullOrEmpty(ClientId))
        {
            switch (ClientCredentialType)
            {
                case ClientCredentialType.AuthorizationHeader:
                {
                    if (BasicAuthenticationHeaderType.Rfc6749 == AuthenticationHeaderType)
                    {
                        SetBasicAuthenticationOAuth(ClientId, ClientSecret);
                    }
                    else if (BasicAuthenticationHeaderType.Rfc2617 == AuthenticationHeaderType)
                    {
                        SetBasicAuthentication(ClientId, ClientSecret);
                    }
                    else
                    {
                        throw new InvalidOperationException("Unsupported basic authentication header style");
                    }

                    break;
                }

                case ClientCredentialType.PostBody:
                {
                    Parameters.AddRequired(OidcConstants.TokenRequest.ClientId, ClientId);
                    Parameters.AddOptional(OidcConstants.TokenRequest.ClientSecret, ClientSecret);

                    break;
                }

                default:
                {
                    throw new InvalidOperationException("Unsupported client credential style");
                }
            }
        }

        if (ClientAssertion is { Type: not null, Value: not null })
        {
            if (ClientCredentialType.AuthorizationHeader == ClientCredentialType && false == String.IsNullOrEmpty(ClientId))
            {
                throw new InvalidOperationException("CredentialStyle.AuthorizationHeader and client assertions are not compatible");
            }

            Parameters.AddOptional(OidcConstants.TokenRequest.ClientAssertionType, ClientAssertion.Type);
            Parameters.AddOptional(OidcConstants.TokenRequest.ClientAssertion, ClientAssertion.Value);
        }

        if (false == String.IsNullOrEmpty(Address))
        {
            RequestUri = new Uri(Address, UriKind.RelativeOrAbsolute);
        }

        if (0 < Parameters.Count)
        {
            Content = new FormUrlEncodedContent(Parameters);
        }
    }

    public void SetBasicAuthentication(string userName, string? password)
    {
        Headers.Authorization = new BasicAuthenticationHeaderValue(userName, password);
    }

    public void SetBasicAuthenticationOAuth(string userName, string? password)
    {
        Headers.Authorization = new BasicAuthenticationOAuthHeaderValue(userName, password);
    }

    protected virtual void Apply(ProtocolRequest request)
    {
        RequestUri = request.RequestUri;
        Version = request.Version;
        Method = request.Method;
        Address = request.Address;
        AuthenticationHeaderType = request.AuthenticationHeaderType;
        ClientAssertion = request.ClientAssertion;
        ClientCredentialType = request.ClientCredentialType;
        ClientId = request.ClientId;
        ClientSecret = request.ClientSecret;
            //DPoPProofToken = DPoPProofToken,
        Parameters = new Parameters(Parameters);

        Headers.Clear();

        foreach (var (key, values) in Headers)
        {
            Headers.TryAddWithoutValidation(key, values);
        }

        foreach (var (key, value) in Options)
        {
            Options.TryAdd(key, value);
        }
    }
    
    private void ApplyInternal(ProtocolRequest request) => Apply(request);
}