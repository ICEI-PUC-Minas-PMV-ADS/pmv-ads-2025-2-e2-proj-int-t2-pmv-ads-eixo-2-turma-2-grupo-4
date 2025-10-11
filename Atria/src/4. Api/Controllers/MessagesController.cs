using System.Threading.Tasks;
using Atria.Application.Features.Messages.SendMessage;
using Atria.Application.Features.Messages.GetMessages;
using Atria.Application.Features.Messages.MarkAsRead;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Atria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MessagesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<MessagesController> _logger;

    public MessagesController(IMediator mediator, ILogger<MessagesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Send([FromBody] SendMessageCommand command)
    {
        try
        {
            var senderId = User.FindFirst("id")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(senderId)) return Unauthorized();

            // Ensure sender is taken from token, not from client body
            command.SenderId = senderId;

            var messageId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetMessagesForUser), new { userId = command.SenderId }, new { id = messageId });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid request to send message");
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetMessagesForUser(string userId)
    {
        var query = new GetMessagesQuery { UserId = userId };
        var messages = await _mediator.Send(query);
        return Ok(messages);
    }

    [HttpPost("{id}/read")]
    public async Task<IActionResult> MarkAsRead(string id)
    {
        var userId = User.FindFirst("id")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var cmd = new MarkAsReadCommand { MessageId = id, RequesterId = userId };
        try
        {
            await _mediator.Send(cmd);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Failed to mark message as read");
            return BadRequest(new { error = ex.Message });
        }
    }
}