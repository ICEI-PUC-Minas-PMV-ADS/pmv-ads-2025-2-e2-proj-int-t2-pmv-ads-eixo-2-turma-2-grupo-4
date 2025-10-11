using FluentValidation;

namespace Atria.Application.Features.Users.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id do usuário é obrigatório.");

        RuleFor(x => x.Nome)
            .MaximumLength(255).WithMessage("Nome deve ter no máximo 255 caracteres.");

        RuleFor(x => x.Matricula)
            .MaximumLength(50).WithMessage("Matrícula deve ter no máximo 50 caracteres.");

        RuleFor(x => x.AreaAtuacao)
            .MaximumLength(100).WithMessage("Área de atuação deve ter no máximo 100 caracteres.");

        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.Nome) || !string.IsNullOrWhiteSpace(x.Matricula) || !string.IsNullOrWhiteSpace(x.AreaAtuacao))
            .WithMessage("Pelo menos um campo (Nome, Matrícula ou ÁreaAtuacao) deve ser informado para atualização.");
    }
}