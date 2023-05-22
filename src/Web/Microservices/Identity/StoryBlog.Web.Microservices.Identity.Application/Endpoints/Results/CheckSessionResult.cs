using Microsoft.AspNetCore.Http;
using StoryBlog.Web.Microservices.Identity.Application.Configuration;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Hosting;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;

namespace StoryBlog.Web.Microservices.Identity.Application.Endpoints.Results;

/// <summary>
/// 
/// </summary>
[EmbeddedResource("form", ResourceName = "StoryBlog.Web.Microservices.Identity.Application.Scripts.ChechSession.template.html")]
internal sealed class CheckSessionResult : EmbeddedResourceProvider, IEndpointResult
{
    private IdentityServerOptions? options;
    private readonly SemaphoreSlim gate;
    private static volatile string? FormattedHtml;
    private static volatile string? LastCheckSessionCookieName;

    public CheckSessionResult()
    {
        gate = new SemaphoreSlim(1);
    }

    internal CheckSessionResult(IdentityServerOptions options)
        : this()
    {
        this.options = options;
    }

    public async Task ExecuteAsync(HttpContext context)
    {
        Init(context);

        AddCspHeaders(context);

        var html = await GetHtmlAsync(options.Authentication.CheckSessionCookieName, context.RequestAborted);
        await context.Response.WriteHtmlAsync(html);
    }

    private void Init(HttpContext context)
    {
        options = options ?? context.RequestServices.GetRequiredService<IdentityServerOptions>();
    }

    private void AddCspHeaders(HttpContext context)
    {
        context.Response.AddScriptCspHeaders(options.Csp, "sha256-fa5rxHhZ799izGRP38+h4ud5QXNT0SFaFlh4eqDumBI=");
    }

    private async Task<string> GetHtmlAsync(string cookieName, CancellationToken cancellationToken)
    {
        if (cookieName != LastCheckSessionCookieName)
        {
            await gate.WaitAsync(cancellationToken);

            try
            {
                if (cookieName != LastCheckSessionCookieName)
                {
                    using (var stream = GetResourceStream("form"))
                    {
                        var reader = new StreamReader(stream, Encoding.UTF8);
                        var content = await reader.ReadToEndAsync(cancellationToken);
                        
                        FormattedHtml = content.Replace("{cookieName}", cookieName);
                    }

                    //FormattedHtml = Html.Replace("{cookieName}", cookieName);
                    LastCheckSessionCookieName = cookieName;
                }
            }
            finally
            {
                gate.Release();
            }
        }

        return FormattedHtml;
    }
}