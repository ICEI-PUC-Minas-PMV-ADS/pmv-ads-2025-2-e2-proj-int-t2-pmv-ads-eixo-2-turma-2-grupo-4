using FluentValidation;

namespace Atria.Application.Features.Material.CreateMaterial;

public class CreateMaterialCommandValidator : AbstractValidator<CreateMaterialCommand>
{
    public CreateMaterialCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Author).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Year).GreaterThan(0);
        RuleFor(x => x.TipoMaterial).NotEmpty().Must(t => t == "Livro" || t == "Artigo").WithMessage("TipoMaterial must be 'Livro' or 'Artigo'.");
    }
}