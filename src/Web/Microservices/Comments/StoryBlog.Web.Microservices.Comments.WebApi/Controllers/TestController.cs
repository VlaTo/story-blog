using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoryBlog.Web.Microservices.Comments.Application.Handlers.Test;
using StoryBlog.Web.Microservices.Posts.Shared.Models;

namespace StoryBlog.Web.Microservices.Comments.WebApi.Controllers;

[ApiVersion("1.0-alpha")]
[AllowAnonymous]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class TestController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly ILogger<TestController> logger;

    public TestController(
        IMediator mediator,
        ILogger<TestController> logger)
    {
        this.mediator = mediator;
        this.logger = logger;
    }

    [ProducesResponseType(typeof(PostModel), StatusCodes.Status200OK)]
    [HttpGet("{key:guid:required}")]
    public async Task<IActionResult> GetComment([FromRoute] Guid key)
    {
        var command = new TestCommand(key);
        var result = await mediator.Send(command, HttpContext.RequestAborted);

        if (result.Succeeded)
        {
            return Ok(result.Value);
        }

        return BadRequest();
    }
}