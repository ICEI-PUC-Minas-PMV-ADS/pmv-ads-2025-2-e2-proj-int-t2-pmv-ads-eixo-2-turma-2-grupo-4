namespace Atria.Domain.Entities.MaterialContext;

public class Livro : Material
{
    public string? ISBN { get; set; }
    public string? Editora { get; set; }

    public Livro()
    {
        TipoMaterial = TipoMaterial.Livro;
    }
}