using MediatR;

namespace Atria.Application.Features.Users;

public class GetUserByIdQuery : IRequest<UserDto?>
{
    public string Id { get; init; } = string.Empty;
}