using FluentValidation;

namespace Atria.Application.Features.Users.Register;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .MaximumLength(255)
            .WithMessage("Nome é obrigatório e deve ter no máximo 255 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(150)
            .WithMessage("Email inválido");

        RuleFor(x => x.Senha)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(100)
            .WithMessage("Senha deve ter entre 6 e 100 caracteres");

        RuleFor(x => x.TipoUsuario)
            .IsInEnum()
            .WithMessage("Tipo de usuário inválido");

        When(x => x.TipoUsuario == Domain.Enums.TipoUsuario.Professor, () =>
        {
            RuleFor(x => x.Matricula)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Matrícula é obrigatória para professores e deve ter no máximo 50 caracteres");

            RuleFor(x => x.AreaAtuacao)
                .MaximumLength(100)
                .WithMessage("Área de atuação deve ter no máximo 100 caracteres");
        });
    }
}