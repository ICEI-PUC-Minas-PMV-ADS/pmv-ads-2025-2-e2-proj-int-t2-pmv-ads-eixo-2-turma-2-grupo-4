using MediatR;

namespace Atria.Application.Features.Users.Login;

public class LoginUserCommand : IRequest<string>
{
    public string Email { get; init; } = string.Empty;
    public string Senha { get; init; } = string.Empty;
}