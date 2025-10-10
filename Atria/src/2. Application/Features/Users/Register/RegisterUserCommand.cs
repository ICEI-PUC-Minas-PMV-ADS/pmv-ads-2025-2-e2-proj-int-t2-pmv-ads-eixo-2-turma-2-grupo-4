using System.ComponentModel.DataAnnotations;
using Atria.Domain.Enums;
using MediatR;

namespace Atria.Application.Features.Users.Register;

public class RegisterUserCommand : IRequest<string>
{
    [Required]
    [StringLength(255)]
    public string Nome { get; init; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; init; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Senha { get; init; } = string.Empty;

    [Required]
    public TipoUsuario TipoUsuario { get; init; }

    // Campos opcionais para Professor
    [StringLength(50)]
    public string? Matricula { get; init; }

    [StringLength(100)]
    public string? AreaAtuacao { get; init; }
}