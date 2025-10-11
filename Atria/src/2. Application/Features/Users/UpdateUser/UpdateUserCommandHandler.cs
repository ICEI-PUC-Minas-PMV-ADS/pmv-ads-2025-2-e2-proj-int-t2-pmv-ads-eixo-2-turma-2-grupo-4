using System;
using System.Threading;
using System.Threading.Tasks;
using Atria.Application.Common.Interfaces;
using Atria.Application.Features.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Atria.Application.Features.Users.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == request.Id, cancellationToken);
        if (user == null) throw new ArgumentException("User not found", nameof(request.Id));

        // Only allow owner or moderator/admin (moderator logic: role string == "Moderador")
        var isOwner = request.RequesterId == request.Id;
        var isModerator = string.Equals(request.RequesterRole, "Moderador", StringComparison.OrdinalIgnoreCase);

        if (!isOwner && !isModerator) throw new ArgumentException("Not authorized to update this user");

        if (!string.IsNullOrEmpty(request.Nome)) user.Nome = request.Nome;
        if (!string.IsNullOrEmpty(request.Matricula)) user.Matricula = request.Matricula;
        if (!string.IsNullOrEmpty(request.AreaAtuacao)) user.AreaAtuacao = request.AreaAtuacao;

        await _context.SaveChangesAsync(cancellationToken);

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