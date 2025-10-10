using Atria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Atria.Application.Features.Users;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<GetUserByIdQueryHandler> _logger;

    public GetUserByIdQueryHandler(IApplicationDbContext context, ILogger<GetUserByIdQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.IdUsuario == request.Id, cancellationToken);

        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found", request.Id);
            return null;
        }

        return new UserDto
        {
            IdUsuario = user.IdUsuario,
            Nome = user.Nome,
            Email = user.Email,
            TipoUsuario = user.TipoUsuario,
            DataCadastro = user.DataCadastro,
            Matricula = user.Matricula,
            AreaAtuacao = user.AreaAtuacao
        };
    }
}