using System.Threading.Tasks;
using Atria.Application.Features.Posts.CreatePost;
using Atria.Application.Features.Comments.CreateComment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Atria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<PostsController> _logger;

    public PostsController(IMediator mediator, ILogger<PostsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostCommand command)
    {
        var requesterId = User.FindFirst("id")?.Value;
        if (string.IsNullOrEmpty(requesterId)) return Unauthorized();

        var cmd = new CreatePostCommand
        {
            Title = command.Title,
            Content = command.Content,
            CommunityId = command.CommunityId,
            NoForumGeral = command.NoForumGeral,
            RequesterId = requesterId
        };

        var id = await _mediator.Send(cmd);
        return CreatedAtAction(nameof(GetPost), new { id }, new { id });
    }

    [HttpPost("{postId}/comments")]
    [Authorize]
    public async Task<IActionResult> CreateComment(int postId, [FromBody] CreateCommentCommand command)
    {
        var requesterId = User.FindFirst("id")?.Value;
        if (string.IsNullOrEmpty(requesterId)) return Unauthorized();

        var cmd = new CreateCommentCommand
        {
            PostId = postId,
            Content = command.Content,
            RequesterId = requesterId
        };

        var id = await _mediator.Send(cmd);
        return CreatedAtAction(nameof(GetPost), new { id = postId }, new { id });
    }

    [HttpGet("{id}")]
    public IActionResult GetPost(int id)
    {
        // Not implemented: return placeholder
        return Ok(new { id });
    }
}