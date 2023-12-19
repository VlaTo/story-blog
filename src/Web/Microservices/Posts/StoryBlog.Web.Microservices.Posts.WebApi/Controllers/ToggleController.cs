using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoryBlog.Web.Microservices.Posts.Application.Handlers.TogglePublicity;
using StoryBlog.Web.Microservices.Posts.Shared.Models;

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
    [ProducesResponseType(typeof(PostPublicityResponse), StatusCodes.Status200OK)]
    [HttpPut("{slugOrKey:required}")]
    public async Task<IActionResult> TogglePostPublicity([FromRoute] string slugOrKey, [FromBody] PostPublicityRequest request)
    {
        var command = new TogglePostPublicityCommand(slugOrKey, request.IsPublic, User);
        var result = await mediator.Send(command).ConfigureAwait(false);

        if (result.Succeeded)
        {
            //var model = mapper.Map<PostPublicityResponse>(.Value);
            return Ok(new PostPublicityResponse
            {
                PostKey = result.Value.PostKey,
                IsPublic = result.Value.IsPublic
            });
        }

        logger.LogDebug($"Post not found for key: {slugOrKey}");

        return BadRequest();
    }

}