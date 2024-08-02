using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoryBlog.Web.Microservices.Posts.Shared.Models;

namespace StoryBlog.Web.Microservices.Posts.WebApi.InterService.Controllers;

[ApiVersion("1.0-alpha")]
[Authorize]
[ApiController]
[Route("s2s/v{version:apiVersion}/[controller]")]
public sealed class PostController(TimeProvider timeProvider) : ControllerBase
{
    [ProducesResponseType(typeof(PostModel), StatusCodes.Status200OK)]
    [HttpGet("{key:guid:required}")]
    public Task<IActionResult> GetPost([FromRoute] Guid key)
    {
        var claims = User.Claims;
        var postModel = new PostModel
        {
            Key = key,
            Title = "Lorem ipsum dolor sit amet",
            Author = "Anonymous",
            CommentsCount = 0,
            CreatedAt = timeProvider.GetUtcNow(),
            PublicationStatus = PostPublicationStatus.Pending,
            Slug = "lorem-ipsum-dolor-sit-amet",
            Text = "Lorem ipsum dolor sit amet"
        };

        return Task.FromResult<IActionResult>(Ok(postModel));
    }
}