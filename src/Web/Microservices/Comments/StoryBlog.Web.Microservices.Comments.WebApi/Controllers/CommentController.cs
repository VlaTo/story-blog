using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoryBlog.Web.Microservices.Comments.Shared.Models;
using StoryBlog.Web.Microservices.Comments.WebApi.Core;

namespace StoryBlog.Web.Microservices.Comments.WebApi.Controllers;

/// <summary>
/// Endpoint to manage blog post comment operations.
/// </summary>
[ApiVersion("1.0-alpha")]
[AllowAnonymous]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class CommentController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly ILogger<CommentsController> logger;

    public CommentController(
        IMediator mediator,
        IMapper mapper,
        ILogger<CommentsController> logger)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.logger = logger;
    }

    /// <summary>
    /// Gets comment <see cref="CommentModel" /> with <paramref name="key"/> specified.
    /// </summary>
    /// <param name="key">Comment key to retrieve.</param>
    /// <returns>The <see cref="CommentModel" />.</returns>
    [ProducesResponseType(typeof(CommentModel), StatusCodes.Status200OK)]
    [HttpGet("{key:guid:required}", Name = RouteNames.GetCommentRouteKey)]
    public Task<IActionResult> GetComment([FromRoute] Guid key)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets comment <see cref="CommentModel" /> with <paramref name="key"/> specified.
    /// </summary>
    /// <param name="key">Comment key to retrieve.</param>
    /// <returns>The <see cref="CommentModel" />.</returns>
    //[ProducesResponseType(typeof(CommentModel), StatusCodes.Status200OK)]
    [HttpPut("{key:guid:required}")]
    public Task<IActionResult> UpdateComment([FromRoute] Guid key)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets comment <see cref="CommentModel" /> with <paramref name="key"/> specified.
    /// </summary>
    /// <param name="key">Comment key to retrieve.</param>
    /// <returns>The <see cref="CommentModel" />.</returns>
    //[ProducesResponseType(typeof(CommentModel), StatusCodes.Status200OK)]
    [HttpDelete("{key:guid:required}")]
    public Task<IActionResult> DeleteComment([FromRoute] Guid key)
    {
        throw new NotImplementedException();
    }
}
