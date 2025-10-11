using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Atria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Atria.Application.Features.Communities.ListMembers;

public class ListMembersQueryHandler : IRequestHandler<ListMembersQuery, IEnumerable<MemberDto>>
{
    private readonly IApplicationDbContext _context;

    public ListMembersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MemberDto>> Handle(ListMembersQuery request, CancellationToken cancellationToken)
    {
        var members = await _context.ComunidadeMembros
            .AsNoTracking()
            .Where(m => m.ComunidadeId == request.CommunityId)
            .Include(m => m.Usuario)
            .ToListAsync(cancellationToken);

        return members.Select(m => new MemberDto
        {
            UserId = m.UsuarioId,
            Nome = m.Usuario?.Nome ?? string.Empty,
            IsAdmin = m.IsAdmin,
            IsModAdmin = m.IsModAdmin,
            JoinedAt = m.JoinedAt.ToString("o")
        }).ToList();
    }
}