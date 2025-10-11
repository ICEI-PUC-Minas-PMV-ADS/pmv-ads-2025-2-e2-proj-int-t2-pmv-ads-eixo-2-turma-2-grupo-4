using System.Threading.Tasks;
using Atria.Application.Features.Users.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Atria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        try
        {
            var token = await _mediator.Send(command);
            return Ok(new { token });
        }
        catch (System.ArgumentException ex)
        {
            _logger.LogWarning(ex, "Login failed");
            return BadRequest(new { error = ex.Message });
        }
    }
}