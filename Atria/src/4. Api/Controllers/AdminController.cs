using System.Threading.Tasks;
using Atria.Application.Features.Admin.PromoteUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Atria.Api.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Moderador")] // system moderators only
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AdminController> _logger;

    public AdminController(IMediator mediator, ILogger<AdminController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("promote")]
    public async Task<IActionResult> Promote([FromBody] PromoteUserCommand command)
    {
        // Ensure requester id comes from token
        var requesterId = User.FindFirst("id")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(requesterId)) return Unauthorized();

        var cmd = new PromoteUserCommand
        {
            TargetUserId = command.TargetUserId,
            MakeAdminInCommunity = command.MakeAdminInCommunity,
            CommunityId = command.CommunityId,
            MakeModAdmin = command.MakeModAdmin,
            RequesterId = requesterId
        };

        try
        {
            await _mediator.Send(cmd);
            return NoContent();
        }
        catch (System.ArgumentException ex)
        {
            _logger.LogWarning(ex, "Failed to promote user");
            return BadRequest(new { error = ex.Message });
        }
    }
}