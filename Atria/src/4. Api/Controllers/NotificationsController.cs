using System.Threading.Tasks;
using Atria.Application.Features.Notifications.GetNotifications;
using Atria.Application.Features.Notifications.GetUnread;
using Atria.Application.Features.Notifications.MarkAsRead;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Atria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<NotificationsController> _logger;

    public NotificationsController(IMediator mediator, ILogger<NotificationsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMyNotifications()
    {
        var userId = User.FindFirst("id")?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var query = new GetNotificationsQuery { UserId = userId };
        var notifications = await _mediator.Send(query);
        return Ok(notifications);
    }

    [HttpGet("me/unread")]
    [Authorize]
    public async Task<IActionResult> GetMyUnreadNotifications()
    {
        var userId = User.FindFirst("id")?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var query = new GetUnreadNotificationsQuery { UserId = userId };
        var notifications = await _mediator.Send(query);
        return Ok(notifications);
    }

    [HttpPost("{id}/read")]
    [Authorize]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var userId = User.FindFirst("id")?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var cmd = new MarkNotificationAsReadCommand { NotificationId = id, RequesterId = userId };
        await _mediator.Send(cmd);
        return NoContent();
    }

    [HttpPost("me/read-all")]
    [Authorize]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var userId = User.FindFirst("id")?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var cmd = new MarkAllAsReadCommand { UserId = userId };
        await _mediator.Send(cmd);
        return NoContent();
    }
}