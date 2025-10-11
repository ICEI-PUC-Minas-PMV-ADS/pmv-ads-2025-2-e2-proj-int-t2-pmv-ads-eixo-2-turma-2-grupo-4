using MediatR;

namespace Atria.Application.Features.Material.CreateMaterial;

public class CreateMaterialCommand : IRequest<int>
{
    public string Title { get; init; } = string.Empty;
    public string Author { get; init; } = string.Empty;
    public int Year { get; init; }
    public string Status { get; init; } = "Pendente";
    public string RequesterId { get; init; } = string.Empty; // who creates (must be Professor)
    public string TipoMaterial { get; init; } = string.Empty; // Livro|Artigo
    public string? ISBN { get; init; }
    public string? DOI { get; init; }
    public string? Editora { get; init; }
}