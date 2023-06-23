using System.Net.Mime;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.EntityFrameworkCore.Diagnostics;
using StoryBlog.Web.Microservices.Identity.Application.Handlers.SigningIn;
using StoryBlog.Web.Microservices.Identity.Application.Services;
using StoryBlog.Web.Microservices.Identity.WebApi.ViewModels.Account;
using StoryBlog.Web.Microservices.Identity.WebApi.ViewModels.RedirectPage;

namespace StoryBlog.Web.Microservices.Identity.WebApi.Controllers;

[AllowAnonymous]
[Route("[controller]")]
public class AccountController : Controller
{
    internal const string ReturnUrlKey = "ReturnUrl";
    internal const string ClientDescriptionKey = "ClientDescription";
    internal const string ClientLogoKey = "ClientLogo";

    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IIdentityServerInteractionService identityServerInteraction;
    private readonly ILogger<AccountController> logger;

    public AccountController(
        IMediator mediator,
        IMapper mapper,
        IIdentityServerInteractionService identityServerInteraction,
        ILogger<AccountController> logger)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.identityServerInteraction = identityServerInteraction;
        this.logger = logger;
    }

    // GET: account/login
    [HttpGet("login")]
    public async Task<IActionResult> Login([FromQuery(Name = "returnUrl")]string returnUrl)
    {
        var context = await identityServerInteraction.GetAuthorizationContextAsync(returnUrl);

        if (null != context)
        {
            ViewData[ClientDescriptionKey] = context.Client?.Description;
            ViewData[ClientLogoKey] = context.Client?.LogoUri;
        }

        ViewData[ReturnUrlKey] = returnUrl;

        var model = new SigninFormViewModel
        {
            Email = "guest@storyblog.net",
            Password = "User_guEst1"
        };

        return View(model);
    }

    // POST: account/login
    [HttpPost("login")]
    [Consumes("application/x-www-form-urlencoded")]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([FromForm] SigninFormViewModel model, [FromForm(Name = ReturnUrlKey)]string returnUrl)
    {
        var context = await identityServerInteraction.GetAuthorizationContextAsync(returnUrl);

        if (ModelState.IsValid)
        {
            var command = new SigninCommand(context, model.Email, model.Password, model.RememberMe);
            var result = await mediator.Send(command, HttpContext.RequestAborted);

            if (result.IsOfT2)
            {
                return View("RedirectPage", new RedirectPageViewModel(returnUrl));
            }

            ModelState.AddModelError("General", "General error");
        }

        if (null != context)
        {
            ViewData[ClientDescriptionKey] = context.Client?.Description;
            ViewData[ClientLogoKey] = context.Client?.LogoUri;
        }

        ViewData[ReturnUrlKey] = returnUrl;

        return View(model);
    }

    private IActionResult RedirectToUrl(string url)
    {
        if (false == Url.IsLocalUrl(url))
        {
            return Redirect(url);
        }

        return RedirectToAction(nameof(Login));
    }
}
