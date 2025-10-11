using System.Threading.Tasks;
using Atria.Application.Features.Communities.AddMember;
using Atria.Application.Features.Communities.RemoveMember;
using Atria.Application.Features.Communities.ListMembers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Atria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommunitiesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CommunitiesController> _logger;

    public CommunitiesController(IMediator mediator, ILogger<CommunitiesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("{communityId}/members")]
    [Authorize]
    public async Task<IActionResult> AddMember(int communityId, [FromBody] AddMemberCommand command)
    {
        var requesterId = User.FindFirst("id")?.Value;
        if (string.IsNullOrEmpty(requesterId)) return Unauthorized();

        var cmd = new AddMemberCommand
        {
            CommunityId = communityId,
            UserId = command.UserId,
            IsAdmin = command.IsAdmin,
            IsModAdmin = command.IsModAdmin,
            RequesterId = requesterId
        };

        try
        {
            await _mediator.Send(cmd);
            return NoContent();
        }
        catch (System.ArgumentException ex)
        {
            _logger.LogWarning(ex, "Failed to add member");
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{communityId}/members/{userId}")]
    [Authorize]
    public async Task<IActionResult> RemoveMember(int communityId, string userId)
    {
        var requesterId = User.FindFirst("id")?.Value;
        if (string.IsNullOrEmpty(requesterId)) return Unauthorized();

        var cmd = new RemoveMemberCommand
        {
            CommunityId = communityId,
            UserId = userId,
            RequesterId = requesterId
        };

        try
        {
            await _mediator.Send(cmd);
            return NoContent();
        }
        catch (System.ArgumentException ex)
        {
            _logger.LogWarning(ex, "Failed to remove member");
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{communityId}/members")]
    public async Task<IActionResult> ListMembers(int communityId)
    {
        var query = new ListMembersQuery { CommunityId = communityId };
        var members = await _mediator.Send(query);
        return Ok(members);
    }
}