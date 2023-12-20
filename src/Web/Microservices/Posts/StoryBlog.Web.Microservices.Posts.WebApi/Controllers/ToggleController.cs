using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoryBlog.Web.Common.Domain.Entities;
using StoryBlog.Web.Microservices.Posts.Application.Handlers.TogglePublicity;
using StoryBlog.Web.Microservices.Posts.Shared.Models;
using StoryBlog.Web.Microservices.Posts.WebApi.Extensions;

namespace StoryBlog.Web.Microservices.Posts.WebApi.Controllers;

/// <summary>
/// 
/// </summary>
[ApiVersion("1.0-alpha")]
[AllowAnonymous]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public sealed class ToggleController : Controller
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly ILogger<ToggleController> logger;

    public ToggleController(
        IMediator mediator,
        IMapper mapper,
        ILogger<ToggleController> logger)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.logger = logger;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="slugOrKey"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPut("{slugOrKey:required}")]
    public async Task<IActionResult> TogglePostPublicity([FromRoute] string slugOrKey, [FromBody] PostPublicityRequest request)
    {
        var visibilityStatus = request.VisibilityStatus.AsEnum<VisibilityStatus>();
        var command = new TogglePostPublicityCommand(slugOrKey, visibilityStatus, User);
        var result = await mediator.Send(command).ConfigureAwait(false);

        if (result.Succeeded)
        {
            return Ok();
        }

        logger.LogDebug(result.Error!.Message);

        return BadRequest();
    }

}