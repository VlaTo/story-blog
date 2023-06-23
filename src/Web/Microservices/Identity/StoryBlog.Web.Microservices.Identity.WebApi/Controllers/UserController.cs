using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoryBlog.Web.Microservices.Identity.Application.Handlers.CreateUser;

namespace StoryBlog.Web.Microservices.Identity.WebApi.Controllers;

[AllowAnonymous]
[Route("[controller]")]
public class UserController : Controller
{
    private readonly IMediator mediator;

    public UserController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateNewUser()
    {
        var command = new CreateUserCommand("guest@storyblog.net", "User_guEst1");
        var result = await mediator.Send(command, HttpContext.RequestAborted);

        if (result.Succeeded)
        {
            return Ok();
        }

        return BadRequest();
    }
}