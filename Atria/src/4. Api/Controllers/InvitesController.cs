using System.Threading.Tasks;
using Atria.Application.Features.Communities.InviteMember;
using Atria.Application.Features.Communities.AcceptInvite;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Atria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvitesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<InvitesController> _logger;

    public InvitesController(IMediator mediator, ILogger<InvitesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("{communityId}/invite/{userId}")]
    [Authorize]
    public async Task<IActionResult> Invite(int communityId, string userId)
    {
        var requesterId = User.FindFirst("id")?.Value;
        if (string.IsNullOrEmpty(requesterId)) return Unauthorized();

        var cmd = new InviteMemberCommand
        {
            CommunityId = communityId,
            TargetUserId = userId,
            RequesterId = requesterId
        };

        try
        {
            await _mediator.Send(cmd);
            return NoContent();
        }
        catch (System.ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invite failed");
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("accept/{communityId}")]
    [Authorize]
    public async Task<IActionResult> Accept(int communityId)
    {
        var userId = User.FindFirst("id")?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var cmd = new AcceptInviteCommand
        {
            CommunityId = communityId,
            UserId = userId
        };

        try
        {
            await _mediator.Send(cmd);
            return NoContent();
        }
        catch (System.ArgumentException ex)
        {
            _logger.LogWarning(ex, "Accept invite failed");
            return BadRequest(new { error = ex.Message });
        }
    }
}