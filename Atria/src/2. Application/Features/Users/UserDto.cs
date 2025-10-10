using Atria.Domain.Enums;

namespace Atria.Application.Features.Users;

public class UserDto
{
    public string IdUsuario { get; init; } = string.Empty;
    public string Nome { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public TipoUsuario TipoUsuario { get; init; }
    public DateTime DataCadastro { get; init; }
    public string? Matricula { get; init; }
    public string? AreaAtuacao { get; init; }
}