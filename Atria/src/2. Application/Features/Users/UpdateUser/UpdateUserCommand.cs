using MediatR;

namespace Atria.Application.Features.Users.UpdateUser;

public class UpdateUserCommand : IRequest<UserDto>
{
    public string Id { get; init; } = string.Empty;
    public string? Nome { get; init; }
    public string? Matricula { get; init; }
    public string? AreaAtuacao { get; init; }
    public string RequesterId { get; init; } = string.Empty;
    public string? RequesterRole { get; init; }
}