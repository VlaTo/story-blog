using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Application.Hosting;
using System.Net;
using OidcConstants = IdentityModel.OidcConstants;

namespace StoryBlog.Web.Microservices.Identity.Application.Endpoints.Results;

internal sealed class ProtectedResourceErrorResult : IEndpointResult
{
    public string Error
    {
        get;
        private set;
    }

    public string? ErrorDescription
    {
        get;
        private set;
    }

    public ProtectedResourceErrorResult(string error, string? errorDescription = null)
    {
        Error = error;
        ErrorDescription = errorDescription;
    }

    public Task ExecuteAsync(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        context.Response.SetNoCache();

        if (IdentityServerConstants.ProtectedResourceErrorStatusCodes.TryGetValue(Error, out var statusCode))
        {
            context.Response.StatusCode = statusCode;
        }

        if (Error == OidcConstants.ProtectedResourceErrors.ExpiredToken)
        {
            Error = OidcConstants.ProtectedResourceErrors.InvalidToken;
            ErrorDescription = "The access token expired";
        }

        var errorString = new List<string> { "Bearer realm=\"IdentityServer\"", $"error=\"{Error}\"" };

        if (ErrorDescription.IsPresent())
        {
            errorString.Add($"error_description=\"{ErrorDescription}\"");
        }

        context.Response.Headers[HeaderNames.WWWAuthenticate] = new StringValues(errorString.ToArray());

        return Task.CompletedTask;
    }
}