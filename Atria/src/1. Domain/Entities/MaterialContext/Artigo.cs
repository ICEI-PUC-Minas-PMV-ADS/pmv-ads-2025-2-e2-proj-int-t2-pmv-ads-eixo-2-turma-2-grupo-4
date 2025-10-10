namespace Atria.Domain.Entities.MaterialContext;

public class Artigo : Material
{
    public string? DOI { get; set; }

    public Artigo()
    {
        TipoMaterial = TipoMaterial.Artigo;
    }
}