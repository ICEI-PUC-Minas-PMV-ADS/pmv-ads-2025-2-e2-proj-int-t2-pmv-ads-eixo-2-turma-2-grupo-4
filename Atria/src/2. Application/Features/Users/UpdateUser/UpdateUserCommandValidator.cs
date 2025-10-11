using FluentValidation;

namespace Atria.Application.Features.Users.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id do usu�rio � obrigat�rio.");

        RuleFor(x => x.Nome)
            .MaximumLength(255).WithMessage("Nome deve ter no m�ximo 255 caracteres.");

        RuleFor(x => x.Matricula)
            .MaximumLength(50).WithMessage("Matr�cula deve ter no m�ximo 50 caracteres.");

        RuleFor(x => x.AreaAtuacao)
            .MaximumLength(100).WithMessage("�rea de atua��o deve ter no m�ximo 100 caracteres.");

        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.Nome) || !string.IsNullOrWhiteSpace(x.Matricula) || !string.IsNullOrWhiteSpace(x.AreaAtuacao))
            .WithMessage("Pelo menos um campo (Nome, Matr�cula ou �reaAtuacao) deve ser informado para atualiza��o.");
    }
}