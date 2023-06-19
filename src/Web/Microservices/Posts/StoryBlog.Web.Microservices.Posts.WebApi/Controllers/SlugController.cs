using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoryBlog.Web.Microservices.Posts.Application.Handlers.GetPostReference;
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
public class SlugController : Controller
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly ILogger<SlugController> logger;

    public SlugController(
        IMediator mediator,
        IMapper mapper,
        ILogger<SlugController> logger)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.logger = logger;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="slug"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(GeneratedSlugModel), StatusCodes.Status200OK)]
    [HttpGet("{slug:required}")]
    public async Task<IActionResult> GetPostReference([FromRoute] string slug)
    {
        var query = new GetPostReferenceQuery(slug, User);
        var result = await mediator.Send(query).ConfigureAwait(false);

        if (null == result)
        {
            return NotFound();
        }

        return Ok(mapper.Map<PostReferenceModel>(result));
    }
}
