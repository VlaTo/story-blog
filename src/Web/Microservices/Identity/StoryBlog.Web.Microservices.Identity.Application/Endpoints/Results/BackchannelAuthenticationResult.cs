using IdentityModel;
using Microsoft.AspNetCore.Http;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Application.Hosting;
using StoryBlog.Web.Microservices.Identity.Application.ResponseHandling.Models;

namespace StoryBlog.Web.Microservices.Identity.Application.Endpoints.Results;

internal sealed class BackchannelAuthenticationResult : IEndpointResult
{
    public BackchannelAuthenticationResponse Response
    {
        get;
        set;
    }

    public BackchannelAuthenticationResult(BackchannelAuthenticationResponse response)
    {
        Response = response ?? throw new ArgumentNullException(nameof(response));
    }

    public async Task ExecuteAsync(HttpContext context)
    {
        context.Response.SetNoCache();

        if (Response.IsError)
        {
            switch (Response.Error)
            {
                case OidcConstants.BackchannelAuthenticationRequestErrors.InvalidClient:
                {
                    context.Response.StatusCode = 401;
                    break;
                }

                case OidcConstants.BackchannelAuthenticationRequestErrors.AccessDenied:
                {
                    context.Response.StatusCode = 403;
                    break;
                }

                default:
                {
                    context.Response.StatusCode = 400;
                    break;
                }
            }

            var error = new ErrorResultDto
            {
                error = Response.Error,
                error_description = Response.ErrorDescription
            };
            
            await context.Response.WriteJsonAsync(error);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status200OK;

            var success = new SuccessResultDto
            {
                auth_req_id = Response.AuthenticationRequestId,
                expires_in = (int)Response.ExpiresIn.TotalSeconds,
                interval = (int)Response.Interval.TotalSeconds
            };

            await context.Response.WriteJsonAsync(success);
        }
    }

    internal class SuccessResultDto
    {
#pragma warning disable IDE1006 // Naming Styles
        public string auth_req_id { get; set; }
        public int expires_in { get; set; }
        public int interval { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }

    internal class ErrorResultDto
    {
#pragma warning disable IDE1006 // Naming Styles
        public string error { get; set; }
        public string error_description { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }
}