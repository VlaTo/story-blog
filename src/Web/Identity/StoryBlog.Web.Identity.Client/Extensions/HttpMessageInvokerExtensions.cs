using System.Net.Http.Headers;
using StoryBlog.Web.Identity.Client.Requests;
using StoryBlog.Web.Identity.Client.Responses;

namespace StoryBlog.Web.Identity.Client.Extensions;

public static class HttpMessageInvokerExtensions
{
    public static async Task<DiscoveryDocumentResponse> GetDiscoveryDocumentAsync(
        this HttpMessageInvoker invoker,
        DiscoveryDocumentRequest request,
        CancellationToken cancellationToken)
    {
        string address;

        if (false == String.IsNullOrEmpty(request.Address))
        {
            address = request.Address;
        }
        else if (invoker is HttpClient httpClient)
        {
            address = httpClient.BaseAddress!.AbsoluteUri;
        }
        else
        {
            throw new ArgumentNullException(nameof(address));
        }

        var result = DiscoveryEndpoint.ParseUrl(address, request.Policy.DiscoveryDocumentPath);
        var authority = result.Authority;
        var url = result.Url;

        if (String.IsNullOrEmpty(request.Policy.Authority))
        {
            request.Policy.Authority = authority;
        }

        var jwkUrl = String.Empty;

        if (false == DiscoveryEndpoint.IsSecureScheme(new Uri(url), request.Policy))
        {
            return ProtocolResponse.FromException<DiscoveryDocumentResponse>(
                new InvalidOperationException("HTTPS required"),
                $"Error connecting to {url}. HTTPS required."
            );
        }

        try
        {
            var documentRequest = new DiscoveryDocumentRequest(request)
            {
                Method = HttpMethod.Get
            }; // request.Clone();

            //clone.Method = HttpMethod.Get;
            documentRequest.Prepare();

            documentRequest.RequestUri = new Uri(url);

            var response = await invoker.SendAsync(documentRequest, cancellationToken).ConfigureAwait(false);

            if (false == response.IsSuccessStatusCode)
            {
                return await ProtocolResponse
                    .FromHttpResponseAsync<DiscoveryDocumentResponse>(response,
                        $"Error connecting to {url}: {response.ReasonPhrase}")
                    .ConfigureAwait(false);
            }

            var disco = await ProtocolResponse
                .FromHttpResponseAsync<DiscoveryDocumentResponse>(response, request.Policy)
                .ConfigureAwait(false);

            if (disco.IsError)
            {
                return disco;
            }

            try
            {
                jwkUrl = disco.JwksUri;

                if (null != jwkUrl)
                {
                    var webKeySetRequest = new JsonWebKeySetRequest(request)
                    {
                        Method = HttpMethod.Get,
                        Address = jwkUrl
                    };

                    // request.Clone<JsonWebKeySetRequest>();

                    //webKeySetRequest.Method = HttpMethod.Get;
                    //webKeySetRequest.Address = jwkUrl;

                    webKeySetRequest.Prepare();

                    var jwkResponse = await invoker
                        .GetJsonWebKeySetAsync(webKeySetRequest, cancellationToken)
                        .ConfigureAwait(false);

                    if (jwkResponse.IsError)
                    {
                        if (null != jwkResponse.Exception)
                        {
                            return ProtocolResponse.FromException<DiscoveryDocumentResponse>(
                                jwkResponse.Exception,
                                jwkResponse.Error
                            );
                        }

                        if (null != jwkResponse.HttpResponse)
                        {
                            return await ProtocolResponse
                                .FromHttpResponseAsync<DiscoveryDocumentResponse>(
                                    jwkResponse.HttpResponse,
                                    $"Error connecting to {jwkUrl}: {jwkResponse.HttpErrorReason}"
                                )
                                .ConfigureAwait(false);
                        }
                        // If IsError is true, but we have neither an Exception nor an HttpResponse, something very weird is going on
                        // I don't think it is actually possible for this to occur, but just in case...

                        return ProtocolResponse.FromException<DiscoveryDocumentResponse>(
                            new ArgumentNullException(nameof(jwkResponse.HttpResponse)),
                            "Unknown error retrieving JWKS - neither an exception nor an HttpResponse is available"
                        );
                    }

                    disco.KeySet = jwkResponse.KeySet;
                }

                return disco;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return ProtocolResponse.FromException<DiscoveryDocumentResponse>(ex,
                    $"Error connecting to {jwkUrl}. {ex.Message}.");
            }
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            return ProtocolResponse.FromException<DiscoveryDocumentResponse>(ex,
                $"Error connecting to {url}. {ex.Message}.");
        }
    }

    public static async Task<JsonWebKeySetResponse> GetJsonWebKeySetAsync(
        this HttpMessageInvoker client,
        string? address,
        CancellationToken cancellationToken)
    {
        var request = new JsonWebKeySetRequest
        {
            Address = address
        };

        return await client.GetJsonWebKeySetAsync(request, cancellationToken).ConfigureAwait(false);
    }

    public static async Task<JsonWebKeySetResponse> GetJsonWebKeySetAsync(
        this HttpMessageInvoker client,
        JsonWebKeySetRequest request,
        CancellationToken cancellationToken)
    {
        var clone = request.Clone();

        clone.Method = HttpMethod.Get;
        clone.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/jwk-set+json"));

        clone.Prepare();

        HttpResponseMessage response;

        try
        {
            response = await client.SendAsync(clone, cancellationToken).ConfigureAwait(false);

            if (false == response.IsSuccessStatusCode)
            {
                return await ProtocolResponse
                    .FromHttpResponseAsync<JsonWebKeySetResponse>(response, $"Error connecting to {clone.RequestUri!.AbsoluteUri}: {response.ReasonPhrase}")
                    .ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            return ProtocolResponse.FromException<JsonWebKeySetResponse>(ex, $"Error connecting to {clone.RequestUri!.AbsoluteUri}. {ex.Message}.");
        }

        return await ProtocolResponse
            .FromHttpResponseAsync<JsonWebKeySetResponse>(response)
            .ConfigureAwait(false);
    }

    public static async Task<TokenResponse> RequestClientCredentialsTokenAsync(
        this HttpMessageInvoker invoker,
        ClientCredentialsTokenRequest request,
        CancellationToken cancellationToken)
    {
        var tokenRequest = new ClientCredentialsTokenRequest(request);

        tokenRequest.Parameters.AddRequired(OidcConstants.TokenRequest.GrantType, OidcConstants.GrantTypes.ClientCredentials);
        tokenRequest.Parameters.AddOptional(OidcConstants.TokenRequest.Scope, request.Scope);

        foreach (var resource in request.Resource)
        {
            tokenRequest.Parameters.AddRequired(OidcConstants.TokenRequest.Resource, resource, allowDuplicates: true);
        }

        return await invoker
            .RequestTokenAsync(tokenRequest, cancellationToken)
            .ConfigureAwait(false);
    }

    public static async Task<TokenResponse> RequestTokenAsync(
        this HttpMessageInvoker invoker,
        TokenRequest request,
        CancellationToken cancellationToken)
    {
        var clone = request.Clone();

        if (!clone.Parameters.ContainsKey(OidcConstants.TokenRequest.GrantType))
        {
            clone.Parameters.AddRequired(OidcConstants.TokenRequest.GrantType, request.GrantType);
        }

        return await invoker
            .RequestTokenAsync(clone, cancellationToken)
            .ConfigureAwait(false);
    }

    internal static async Task<TokenResponse> RequestTokenAsync(
        this HttpMessageInvoker invoker,
        ProtocolRequest request,
        CancellationToken cancellationToken)
    {
        request.Prepare();

        request.Method = HttpMethod.Post;

        try
        {
            var response = await invoker
                .SendAsync(request, cancellationToken)
                .ConfigureAwait(false);
            return await ProtocolResponse
                .FromHttpResponseAsync<TokenResponse>(response)
                .ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            return ProtocolResponse.FromException<TokenResponse>(ex);
        }
    }
}