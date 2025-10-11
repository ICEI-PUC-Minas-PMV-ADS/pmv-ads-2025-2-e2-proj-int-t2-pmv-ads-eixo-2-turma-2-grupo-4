using System.Threading.Tasks;
using Atria.Application.Features.Users;
using Atria.Application.Features.Users.Register;
using Atria.Application.Features.Users.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Atria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IMediator mediator, ILogger<UsersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Register([FromBody] RegisterUserCommand command)
    {
        try
        {
            var userId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = userId }, userId);
        }
        catch (FluentValidation.ValidationException ex)
        {
            _logger.LogWarning("Validation failed during user registration: {Message}", ex.Message);
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while registering user");
            return StatusCode(500, new { error = "An unexpected error occurred while processing your request." });
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetById(string id)
    {
        var query = new GetUserByIdQuery { Id = id };
        var user = await _mediator.Send(query);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetMe()
    {
        var userId = User.FindFirst("id")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var query = new GetUserByIdQuery { Id = userId };
        var user = await _mediator.Send(query);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateUserCommand command)
    {
        var requesterId = User.FindFirst("id")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var requesterRole = User.FindFirst(ClaimTypes.Role)?.Value;

        var cmd = new UpdateUserCommand
        {
            Id = id,
            Nome = command.Nome,
            Matricula = command.Matricula,
            AreaAtuacao = command.AreaAtuacao,
            RequesterId = requesterId ?? string.Empty,
            RequesterRole = requesterRole
        };

        try
        {
            var updated = await _mediator.Send(cmd);
            return Ok(updated);
        }
        catch (FluentValidation.ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation failed during user update: {Message}", ex.Message);
            return BadRequest(new { error = ex.Message });
        }
        catch (System.ArgumentException ex)
        {
            _logger.LogWarning(ex, "Failed to update user");
            return BadRequest(new { error = ex.Message });
        }
    }
}