using FluentValidation;

namespace Atria.Application.Features.Users.Register;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .MaximumLength(255)
            .WithMessage("Nome � obrigat�rio e deve ter no m�ximo 255 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(150)
            .WithMessage("Email inv�lido");

        RuleFor(x => x.Senha)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(100)
            .WithMessage("Senha deve ter entre 6 e 100 caracteres");

        RuleFor(x => x.TipoUsuario)
            .IsInEnum()
            .WithMessage("Tipo de usu�rio inv�lido");

        When(x => x.TipoUsuario == Domain.Enums.TipoUsuario.Professor, () =>
        {
            RuleFor(x => x.Matricula)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Matr�cula � obrigat�ria para professores e deve ter no m�ximo 50 caracteres");

            RuleFor(x => x.AreaAtuacao)
                .MaximumLength(100)
                .WithMessage("�rea de atua��o deve ter no m�ximo 100 caracteres");
        });
    }
}