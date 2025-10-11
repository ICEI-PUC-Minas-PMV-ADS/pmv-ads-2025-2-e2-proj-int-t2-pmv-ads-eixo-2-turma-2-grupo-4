using MediatR;

namespace Atria.Application.Features.Material.ApproveMaterial;

public class ApproveMaterialCommand : IRequest<Unit>
{
    public int MaterialId { get; init; }
    public string RequesterId { get; init; } = string.Empty;
    public bool Approve { get; init; } = true;
}