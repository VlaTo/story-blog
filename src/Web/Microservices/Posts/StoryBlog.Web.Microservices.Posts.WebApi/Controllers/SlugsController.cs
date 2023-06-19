using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoryBlog.Web.Microservices.Posts.Application.Handlers.GenerateSlug;
using StoryBlog.Web.Microservices.Posts.Shared.Models;

namespace StoryBlog.Web.Microservices.Posts.WebApi.Controllers;

/// <summary>
/// 
/// </summary>
//[ApiVersion("1.0-alpha")]
[AllowAnonymous]
[ApiController]
//[Route("api/v{version:apiVersion}/[controller]")]
[Route("api/v1.0-alpha/[controller]")]
public class SlugsController : Controller
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly ILogger<SlugsController> logger;

    public SlugsController(
        IMediator mediator,
        IMapper mapper,
        ILogger<SlugsController> logger)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.logger = logger;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(GeneratedSlugModel), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<IActionResult> GenerateSlug([FromQuery] string title)
    {
        var query = new GenerateSlugQuery(title);
        var result = await mediator.Send(query).ConfigureAwait(false);

        return Ok(new GeneratedSlugModel
        {
            Title = query.Title,
            Slug = result!
        });
    }
}
