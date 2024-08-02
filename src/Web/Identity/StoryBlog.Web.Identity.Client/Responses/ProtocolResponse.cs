using System.Net;
using System.Text.Json;

namespace StoryBlog.Web.Identity.Client.Responses;

public class ProtocolResponse
{
    public HttpResponseMessage? HttpResponse
    {
        get; 
        protected set;
    }

    public string? Raw
    {
        get; 
        protected set;
    }

    public JsonElement? Json
    {
        get; 
        protected set;
    }

    public Exception? Exception
    {
        get; 
        protected set;
    }

    public bool IsError => false == String.IsNullOrEmpty(Error) || ResponseErrorType.None != ErrorType;

    public ResponseErrorType ErrorType
    {
        get; 
        protected set;
    } = ResponseErrorType.None;

    public HttpStatusCode HttpStatusCode => this.HttpResponse?.StatusCode ?? default(HttpStatusCode);

    public string? HttpErrorReason => this.HttpResponse?.ReasonPhrase ?? default;

    public string? Error
    {
        get
        {
            if (false == String.IsNullOrEmpty(ErrorMessage))
            {
                return ErrorMessage;
            }

            if (ResponseErrorType.Http == ErrorType)
            {
                return HttpErrorReason;
            }
            
            if (ResponseErrorType.Exception == ErrorType)
            {
                return Exception!.Message;
            }

            return GetStringOrDefault(OidcConstants.TokenResponse.Error);
        }
    }

    protected string? ErrorMessage
    {
        get; 
        set;
    }

    public static TResponse FromException<TResponse>(Exception ex, string? errorMessage = null)
        where TResponse : ProtocolResponse, new()
    {
        var response = new TResponse
        {
            Exception = ex,
            ErrorType = ResponseErrorType.Exception,
            ErrorMessage = errorMessage
        };

        return response;
    }

    public static async Task<TResponse> FromHttpResponseAsync<TResponse>(
        HttpResponseMessage httpResponse,
        object? initializationData = null,
        bool skipJson = false)
        where TResponse : ProtocolResponse, new()
    {
        var response = new TResponse
        {
            HttpResponse = httpResponse
        };

        // try to read content
        var content = String.Empty;

        try
        {
            // In .NET, empty content is represented in an HttpResponse with the EmptyContent type,
            // the Content property is not nullable, and ReadAsStringAsync returns the empty string.
            //
            // BUT, in .NET Framework, empty content is represented with a null, and attempting to
            // call ReadAsStringAsync would throw a NRE.
            content = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            response.Raw = content;
        }
        catch (Exception ex)
        {
            response.ErrorType = ResponseErrorType.Exception;
            response.Exception = ex;
        }

        // some HTTP error - try to parse body as JSON but allow non-JSON as well
        if (false == httpResponse.IsSuccessStatusCode &&
            HttpStatusCode.BadRequest != httpResponse.StatusCode)
        {
            response.ErrorType = ResponseErrorType.Http;

            if (false == skipJson && false == String.IsNullOrEmpty(content))
            {
                try
                {
                    response.Json = JsonDocument.Parse(content).RootElement;
                }
                catch
                {
                }
            }

            await response.InitializeAsync(initializationData).ConfigureAwait(false);

            return response;
        }

        if (HttpStatusCode.BadRequest == httpResponse.StatusCode)
        {
            response.ErrorType = ResponseErrorType.Protocol;
        }

        // either 200 or 400 - both cases need a JSON response (if present), otherwise error
        try
        {
            if (false == skipJson && false == String.IsNullOrEmpty(content))
            {
                response.Json = JsonDocument.Parse(content).RootElement;
            }
        }
        catch (Exception ex)
        {
            response.ErrorType = ResponseErrorType.Exception;
            response.Exception = ex;
        }

        if (httpResponse.Headers.TryGetValues(OidcConstants.HttpHeaders.DPoPNonce, out var nonceHeaders))
        {
            if (1 == nonceHeaders.Count())
            {
                //response.DPoPNonce = nonceHeaders.First();
            }
        }

        if (false == skipJson)
        {
            await response.InitializeAsync(initializationData).ConfigureAwait(false);
        }

        return response;
    }

    public JsonElement? GetPropertyOrDefault(string name)
    {
        if (Json?.TryGetProperty(name, out var property) ?? false)
        {
            return property;
        }

        return null;
    }

    public string? GetStringOrDefault(string name) => GetPropertyOrDefault(name)?.GetString();

    protected virtual Task InitializeAsync(object? initializationData = null)
    {
        return Task.CompletedTask;
    }
}