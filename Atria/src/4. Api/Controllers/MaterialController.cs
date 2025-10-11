using System.Threading.Tasks;
using Atria.Application.Features.Material.CreateMaterial;
using Atria.Application.Features.Material.ApproveMaterial;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Atria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaterialController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<MaterialController> _logger;

    public MaterialController(IMediator mediator, ILogger<MaterialController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Roles = "Professor")]
    public async Task<IActionResult> Create([FromBody] CreateMaterialCommand command)
    {
        var requesterId = User.FindFirst("id")?.Value;
        if (string.IsNullOrEmpty(requesterId)) return Unauthorized();

        var cmd = new CreateMaterialCommand
        {
            Title = command.Title,
            Author = command.Author,
            Year = command.Year,
            TipoMaterial = command.TipoMaterial,
            ISBN = command.ISBN,
            DOI = command.DOI,
            Editora = command.Editora,
            RequesterId = requesterId,
            CommunityId = command.CommunityId
        };

        var id = await _mediator.Send(cmd);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPost("{id}/approve")]
    [Authorize(Roles = "Moderador")]
    public async Task<IActionResult> Approve(int id, [FromBody] ApproveMaterialCommand command)
    {
        var requesterId = User.FindFirst("id")?.Value;
        if (string.IsNullOrEmpty(requesterId)) return Unauthorized();

        var cmd = new ApproveMaterialCommand
        {
            MaterialId = id,
            RequesterId = requesterId,
            Approve = command.Approve
        };

        await _mediator.Send(cmd);
        return NoContent();
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        // For brevity not implemented yet
        return Ok(new { id });
    }
}